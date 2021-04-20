using System.Text;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CotacolApp.Pages.admin
{
    public class DownloadData : PageModel
    {
        private readonly ICotacolClient _cotacolClient;
        private readonly IUserProfileManager _userProfileManager;

        public DownloadData(ICotacolClient cotacolClient, IUserProfileManager userProfileManager)
        {
            _cotacolClient = cotacolClient;
            _userProfileManager = userProfileManager;
        }

        public async Task<IActionResult> OnGet(string name)
        {
            if (_userProfileManager?.IsAdmin ?? false)
            {
                // do your magic here
                var climbs = await _cotacolClient.GetClimbDataAsync();
                var builder = new StringBuilder();
                builder.AppendLine("Id,Name,City,Province,Surface,AvgGrade,ElevationDiff,Distance,Points");
                foreach (var col in climbs)
                {
                    builder.AppendLine($"{col.Id},{col.Name},{col.City},{col.Province},{col.Surface},{col.AvgGrade},{col.ElevationDiff},{col.Distance},{col.CotacolPoints}");
                }
                
                return File(Encoding.Unicode.GetBytes(builder.ToString()), "application/octet-stream", name);
            }

            return Unauthorized();
        }
    }
}