using API_TEST.Models;
using API_TEST.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaServices mediaServices;

        public MediaController(IMediaServices mediaServices)
        {
            this.mediaServices = mediaServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedia([FromForm] MediaRequest request)
        {
            string? result = await mediaServices.CreateMedia(request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedia(int id, [FromForm] MediaRequest request)
        {
            string? result = await mediaServices.UpdateMedia(id, request);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMedia(int id)
        {
            string? result = mediaServices.DeleteMedia(id);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok("Success");
        }

        [HttpGet("{id}")]
        public IActionResult GetMedia(int id)
        {
            MediaRespone media = mediaServices.GetMedia(id);
            return Ok(media);
        }

        [HttpGet]
        public IActionResult GetListMedia()
        {
            List<MediaRespone> medias = mediaServices.GetListMedia();
            return Ok(medias);
        }
    }
}
