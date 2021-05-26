using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Models.Identity;
using CotacolApp.Services.Extensions;
using CotacolApp.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CotacolApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<CotacolUser> _signInManager;
        private readonly UserManager<CotacolUser> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;
        private ICotacolClient _cotacolClient;
        private IUserProfileManager _profileManager;

        public ExternalLoginModel(
            SignInManager<CotacolUser> signInManager,
            IHttpContextAccessor contextAccessor,
            UserManager<CotacolUser> userManager,
            ILogger<ExternalLoginModel> logger,
            ICotacolClient cotacolClient,
            ICotacolUserClient cotacolUserClient,
            IUserProfileManager profileManager,
            IOptions<CotacolApiSettings> apiSettings,
            ISessionStorageService sessionStorage,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _cotacolClient = cotacolClient;
            _profileManager = profileManager;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required] public string UserName { get; set; }
            [Required] [EmailAddress] public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new {returnUrl});
            _logger.LogInformation($"OAuth login requested and configured URL {redirectUrl}");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            _logger.LogInformation("Forceprompt par: " + Request.Form["ForcePrompt"]);
            if (Request.Form["ForcePrompt"].Contains("true"))
            {
                properties.Items.Add(StravaAuthenticationProperties.ApprovalPrompt, "true");
            }

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl});
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            //var info = await GetLoginInfo(User);

            if (info != null)
            {
                // Sign in the user with this external login provider if the user already has a login.
                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                    isPersistent: false, bypassTwoFactor: true);


                if (result.Succeeded)
                {
                    _logger.LogInformation("{Name} logged in with {LoginProvider} provider.",
                        info.Principal.Identity.Name,
                        info.LoginProvider);
                    await PersistStravaClaimsAsync(info, info.GetUserId());
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    _logger.LogInformation($"User {info.Principal.GetUserName()} logged in & does not have an account");
                    // If the user does not have an account, then ask the user to create an account.
                    ReturnUrl = returnUrl;
                    // AccessTokens
                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
                    _logger.LogInformation($"New user tokens received {accessToken} - {refreshToken}");

                    ProviderDisplayName = info.ProviderDisplayName;
                    var userName = info.GetUserName();
                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = "Your strava user";
                    }

                    Input = new InputModel
                    {
                        UserName = userName
                    };
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                    {
                        Input.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    }
                }

                return Page();
            }
            else
            {
                return Redirect("/StravaBusy");

            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            //var info = await GetLoginInfo(User);

            if (ModelState.IsValid)
            {
                var user = await info.GetUser();
                user.UserName = Input.UserName;
                user.Email = Input.Email;

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var props = new AuthenticationProperties();
                        props.StoreTokens(info.AuthenticationTokens);
                        props.IsPersistent = true;

                        await _signInManager.SignInAsync(user, props);

                        await CreateCotacolUserInBackendAsync(user, info);

                        _logger.LogInformation(
                            $"User {user.UserName} sign in result: {_signInManager.IsSignedIn(info.Principal)}");
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = "Strava";
            ReturnUrl = returnUrl;
            return Page();
        }


        private async Task CreateCotacolUserInBackendAsync(CotacolUser user, ExternalLoginInfo info)
        {
            var rights = await PersistStravaClaimsAsync(info, user.Id);

            // register user here
            if (_cotacolClient != null)
            {
                var request = new UserSetupRequest
                {
                    CotacolHunter = true,
                    UserId = user.Id,
                    StravaRefreshToken = rights.RefreshToken,
                    UserName = user.UserName,
                    FullName = $"{user.FirstName} {user.LastName}",
                    EmailAddress = user.Email,
                    PremiumUser = false,
                    UpdateActivityDescription = rights.UpdateActivity
                };

                await _cotacolClient.SetupUserAsync(request);
            }
        }

        private async Task<StravaClaimSettings> PersistStravaClaimsAsync(ExternalLoginInfo info, string userId)
        {
            var updateActivity = info.AuthenticationTokens
                .FirstOrDefault(t => t.Name.Equals(IdentityExtensions.ActivityWriteClaim))?.Value ?? "false";
            var updateProfile = info.AuthenticationTokens
                .FirstOrDefault(t => t.Name.Equals(IdentityExtensions.ProfileUpdateClaim))?.Value ?? "false";
            var refreshToken = info.AuthenticationTokens.FirstOrDefault(t => t.Name.Equals("refresh_token"))?.Value;
            return await _profileManager.AddStravaClaims(
                updateActivity.Equals("true", StringComparison.InvariantCultureIgnoreCase),
                updateProfile.Equals("true", StringComparison.InvariantCultureIgnoreCase), refreshToken, userId);
        }
    }
}