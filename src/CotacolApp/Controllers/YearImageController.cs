using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CotacolApp.Interfaces;
using CotacolApp.Models.CotacolApi;
using Microsoft.AspNetCore.Mvc;

namespace CotacolApp.Controllers
{
    [ApiController]
    public class YearImageController : ControllerBase
    {
        private IEnumerable<IYearImageGenerator> _imageGenerators;
        private ICotacolUserClient _userClient;

        public YearImageController(IEnumerable<IYearImageGenerator> imageGenerators, ICotacolUserClient userClient)
        {
            _imageGenerators = imageGenerators;
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
            }

            try
            {
                var experimental = Request.Query.ContainsKey("experimental");
                var imageGenerator = _imageGenerators.FirstOrDefault(ig=>ig.IsExperimental==experimental);
                var imageContent = await imageGenerator.CreateImageAsync(summary);
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