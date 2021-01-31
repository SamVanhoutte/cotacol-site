using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Models.Identity;
using CotacolApp.Services;
using CotacolApp.Settings;
using Flurl;
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
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private IHttpContextAccessor _contextAccessor;
        private ICotacolClient _cotacolClient;
        private CotacolApiSettings _apiSettings;

        public ExternalLoginModel(
            SignInManager<CotacolUser> signInManager,
            IHttpContextAccessor contextAccessor,
            UserManager<CotacolUser> userManager,
            ILogger<ExternalLoginModel> logger,
            ICotacolClient cotacolClient,
            IOptions<CotacolApiSettings> apiSettings,
            IEmailSender emailSender)
        {
            _contextAccessor = contextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _cotacolClient = cotacolClient;
            _apiSettings = apiSettings.Value;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public class InputModel
        {
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
            // if (!string.IsNullOrEmpty(_apiSettings?.RedirectDomain))
            // {
            //     _logger.LogInformation($"Redirect domain configured with value '{_apiSettings?.RedirectDomain}'");
            //     var fullUrl = Url.PageLink("./ExternalLogin", pageHandler: "Callback", values: new {returnUrl});
            //
            //     // Remove trailing slash of domain
            //     if (_apiSettings.RedirectDomain.EndsWith("/"))
            //         _apiSettings.RedirectDomain =
            //             _apiSettings.RedirectDomain.Remove(_apiSettings.RedirectDomain.Length - 1, 1);
            //     // Ensure leading slash of relative url
            //     if (!redirectUrl.StartsWith("/")) redirectUrl = "/" + redirectUrl;
            //     // Stitch together
            //     redirectUrl = _apiSettings.RedirectDomain + redirectUrl;
            // }
            _logger.LogInformation($"OAuth login requested and configured URL {redirectUrl}");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
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

            var info = await User.GetLoginInfo(_signInManager);

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name,
                    info.LoginProvider);
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
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await User.GetLoginInfo(_signInManager);

            if (ModelState.IsValid)
            {
                var user = new CotacolUser
                {
                    Id = info.GetUserId(), UserName = info.GetUserName(), Email = Input.Email,
                    FirstName = info.Principal.GetClaim(IdentityExtensions.FirstNameClaim), LastName = info.Principal.GetClaim(IdentityExtensions.LastNameClaim),
                    ProfilePicture = info.Principal.GetClaim(IdentityExtensions.ProfilePictureClaim)
                };

                // register user here
                if (_cotacolClient != null)
                {
                    _logger.LogTrace($"Setting up user {info.Principal.GetUserId()} on Cotacol API");
                    await _cotacolClient.SetupUserAsync(new UserSetupRequest
                    {
                        CotacolHunter = true,
                        UserId = user.Id,
                        UserName = user.UserName,
                        FullName =
                            $"{user.FirstName} {user.LastName}",
                        EmailAddress = user.Email,
                        PremiumUser = false,
                        StravaRefreshToken = info.AuthenticationTokens
                            .FirstOrDefault(token => token.Name.Equals("refresh_token"))?.Value
                    });
                }

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

                        await _signInManager.SignInAsync(user, isPersistent: true, info.LoginProvider);
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
    }
}