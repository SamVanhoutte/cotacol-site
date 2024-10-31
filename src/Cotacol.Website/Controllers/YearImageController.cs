using Cotacol.Website.Interfaces;
using Cotacol.Website.Models.CotacolApi;
using Microsoft.AspNetCore.Mvc;

namespace Cotacol.Website.Controllers
{
    [ApiController]
    public class YearImageController : ControllerBase
    {
        private IYearImageGenerator _imageGenerator;
        private ICotacolUserClient _userClient;

        public YearImageController(IYearImageGenerator imageGenerator, ICotacolUserClient userClient)
        {
            _imageGenerator = imageGenerator;
            _userClient = userClient;
        }
        
        [HttpGet]
        [Route("img/year/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            YearReview summary = null;
            try
            {
                var year = DateTime.UtcNow.Month > 8 ? DateTime.UtcNow.Year : DateTime.UtcNow.Year - 1;
                summary = await _userClient.GetYearReviewAsync(userId, year);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                var experimental = Request.Query.ContainsKey("experimental");
                var imageContent = await _imageGenerator.CreateImageAsync(summary);
                return File(imageContent, "image/png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null;

        }
    }
}