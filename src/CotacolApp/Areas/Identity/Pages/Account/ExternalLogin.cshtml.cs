using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Models.Identity;
using CotacolApp.Services;
using CotacolApp.Services.Extensions;
using CotacolApp.Settings;
using Flurl;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CotacolApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<CotacolUser> _signInManager;
        private readonly UserManager<CotacolUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private IHttpContextAccessor _contextAccessor;
        private ICotacolClient _cotacolClient;
        private CotacolApiSettings _apiSettings;
        private IUserProfileManager _profileManager;
        private ICotacolUserClient _cotacolUserClient;
        private ISessionStorageService _sessionStorage;

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
            _contextAccessor = contextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _cotacolClient = cotacolClient;
            _cotacolUserClient = cotacolUserClient;
            _apiSettings = apiSettings.Value;
            _profileManager = profileManager;
            _sessionStorage = sessionStorage;
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

            var info = await GetLoginInfo(User);

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);


            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name,
                    info.LoginProvider);
                await AddStravaClaims(info, info.GetUserId());
                await _cotacolUserClient.SetUserPermissionsAsync(info.GetPermissions());
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

                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await GetLoginInfo(User);

            if (ModelState.IsValid)
            {
                var user = await info.GetUser();
                user.UserName = Input.UserName;
                user.Email = Input.Email;

                await user.CreateCotacolUser(info, _cotacolClient);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        
                        //TODO : email verification
                        // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        // var callbackUrl = Url.Page(
                        //     "/Account/ConfirmEmail",
                        //     pageHandler: null,
                        //     values: new {area = "Identity", userId = userId, code = code},
                        //     protocol: Request.Scheme);
                        //
                        // await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        //     $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                        //
                        // // If account confirmation is required, we need to show the link if we don't have a real email sender
                        // if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        // {
                        //     return RedirectToPage("./RegisterConfirmation", new {Email = Input.Email});
                        // }
                        // Include the access token in the properties
                        var props = new AuthenticationProperties();
                        props.StoreTokens(info.AuthenticationTokens);
                        props.IsPersistent = true;
                        
                        await _signInManager.SignInAsync(user, props);
                        
                        await AddStravaClaims(info, user.Id);

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
        
        private async Task<ExternalLoginInfo> GetLoginInfo(ClaimsPrincipal user)
        {
            return await _signInManager.GetExternalLoginInfoAsync();

            ExternalLoginInfo info = null;
            if (_signInManager != null)
            {
                info = await _signInManager.GetExternalLoginInfoAsync();
                if (info != null && !info.Principal.Claims.Any())
                {
                    info.Principal = user;
                }
            }


            if (info == null)
            {
                info = new ExternalLoginInfo(user,
                    "Strava", user.GetUserId(),
                    "Strava") {Principal = user};
            }

            return info;
        }

        private async Task AddStravaClaims(ExternalLoginInfo info, string userId)
        {
            // TODO
            var updateActivity = info.AuthenticationTokens.FirstOrDefault(t => t.Name.Equals(IdentityExtensions.ActivityWriteClaim))?.Value ?? "false";
            var updateProfile = info.AuthenticationTokens.FirstOrDefault(t => t.Name.Equals(IdentityExtensions.ProfileUpdateClaim))?.Value ?? "false";
            await _profileManager.AddStravaClaims(updateActivity.Equals("true", StringComparison.InvariantCultureIgnoreCase), 
                updateProfile.Equals("true", StringComparison.InvariantCultureIgnoreCase), userId);
        }

    }
}