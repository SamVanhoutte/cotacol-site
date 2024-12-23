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
        private readonly ILogger<YearImageController> _logger;

        public YearImageController(IYearImageGenerator imageGenerator, ICotacolUserClient userClient, ILogger<YearImageController> logger)
        {
            _imageGenerator = imageGenerator;
            _userClient = userClient;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("img/year/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var showDebug = Request.Query.ContainsKey("debug");
            YearReview summary = null;
            try
            {
                var year = DateTime.UtcNow.Month > 8 ? DateTime.UtcNow.Year : DateTime.UtcNow.Year - 1;
                summary = await _userClient.GetYearReviewAsync(userId, year);
            }
            catch (Exception e)
            {
                if (showDebug)
                {
                    throw;
                }
                _logger.LogError(e, "An error occurred while retrieving year review data");
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
        }
    }
}