using CIR.Core.Entities;
using CIR.Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CIR.Controllers
{
    [Route("api/Image")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly IThumbnailService _thumbnailService;
        public TestController(IThumbnailService thumbnailService)
        {
            _thumbnailService = thumbnailService;
        }

        [HttpPost("Upload")]
        public async Task<ActionResult> Add([FromForm] ImageData imageFile)
        {
            var isThumbnailCreated = _thumbnailService.MakeThumbnailImage(imageFile);
            if(isThumbnailCreated)
            {
                return Ok("Thumbnail created and saved successfully");
            }
            else
            {
                return BadRequest("Some error occurred");
            }
        }
    }
}
