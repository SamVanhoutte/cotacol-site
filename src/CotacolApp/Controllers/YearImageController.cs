using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using Microsoft.AspNetCore.Mvc;

namespace CotacolApp.Controllers
{
    // [Route("api/yearimage")]
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
        [Route("api/year/{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var year = DateTime.UtcNow.Month > 8 ? DateTime.UtcNow.Year : DateTime.UtcNow.Year - 1;
            var summary = await _userClient.GetYearReviewAsync(userId, year);

            var imageContent = await _imageGenerator.CreateImageAsync(summary);
            return File(imageContent, "image/png");
        }



    }
}