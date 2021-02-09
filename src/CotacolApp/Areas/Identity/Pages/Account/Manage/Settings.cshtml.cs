using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using CotacolApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CotacolApp.Areas.Identity.Pages.Account.Manage
{
    public partial class SettingsModel : PageModel
    {
        private readonly UserManager<CotacolUser> _userManager;
        private readonly ICotacolUserClient _cotacolClient;
        private readonly IUserProfileManager _profileManager;

        public SettingsModel(IUserProfileManager profileManager,
            UserManager<CotacolUser> userManager, ICotacolUserClient cotacolClient)
        {
            _profileManager = profileManager;
            _userManager = userManager;
            _cotacolClient = cotacolClient;
        }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Add cotacols to Activity description")]
            public bool UpdateDescription { get; set; }

            [Required]
            [Display(Name = "Enable beta features")]
            public bool EnableBetaFeatures { get; set; }

            [Required]
            [Display(Name = "Private profile")]
            public bool PrivateProfile { get; set; }
        }

        private async Task LoadAsync(CotacolUser user)
        {
            var userSettings = await _cotacolClient.GetProfileAsync();

            Input = new InputModel
            {
                UpdateDescription = userSettings.UserSettings?.UpdateActivityDescription ?? false,
                PrivateProfile = userSettings.UserSettings?.PrivateProfile ?? false,
                EnableBetaFeatures = userSettings.UserSettings?.EnableBetaFeatures ?? false
            };
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.UpdateDescription && !(await _profileManager.CanUpdateDescriptionAsync()))
            {
                StatusMessage = $"Your current Strava authentication does not allow updating activity descriptions.  Please log out and log in again and enable the right permission";
                return RedirectToPage();
            }

            var i = User;
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            try
            {
                await _cotacolClient.UpdateSettingsAsync(new UserSettings
                {
                    CotacolHunter = true, EnableBetaFeatures = Input.EnableBetaFeatures,
                    PrivateProfile = Input.PrivateProfile, UpdateActivityDescription = Input.UpdateDescription
                });
                StatusMessage = "Settings were successfully updated.";
            }
            catch (Exception e)
            {
                StatusMessage = $"Error occurred: {e.Message}";
            }

            return RedirectToPage();
        }
    }
}