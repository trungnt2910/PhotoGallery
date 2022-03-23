using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.Models;
using PhotoGallery.Server.Services;

namespace PhotoGallery.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly ImagesService _imagesService;

        public ImagesController(ImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpGet]
        [Route("list")]
        [AllowAnonymous]
        public async Task<IActionResult> ListImagesAsync()
        {
            try
            {
                var list = await _imagesService.ListImagesAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("favorite")]
        [Authorize]
        public async Task<IActionResult> SetFavoriteAsync([FromBody]FavoriteParams value)
        {
            try
            {
                await _imagesService.SetFavoriteAsync(User.Identity.Name, value);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("favorites")]
        [Authorize]
        public async Task<IActionResult> GetFavoritesAsync()
        {
            try
            {
                var list = await _imagesService.GetFavoritesAsync(User.Identity.Name);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImagesAsync([FromBody]List<string> ids)
        {
            try
            {
                var list = await _imagesService.DeleteImagesAsync(ids);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImageAsync([FromQuery]string id)
        {
            try
            {
                var result = await _imagesService.DeleteImageAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("rename")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RenameImageAsync([FromBody]RenameParams value)
        {
            string id, name;
            (id, name) = (value.Id, value.Name);
            try
            {
                var newImg = await _imagesService.RenameImageAsync(id, name);
                return Ok(newImg);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("upload")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImagesAsync([FromForm] IEnumerable<IFormFile> files)
        {
            const int maxFileSize = 10 * 1024 * 1024;
            var result = new List<Image>();
            foreach (var file in files)
            {
                var filteredName = Path.GetFileNameWithoutExtension(file.FileName);
                try
                {
                    if (file.Length >= maxFileSize)
                    {
                        result.Add(new Image { Name = filteredName, Id = null });
                    }
                    else
                    {
                        result.Add(await _imagesService.UploadImageAsync(file.OpenReadStream(), filteredName, dispose: true));
                    }
                }
                catch
                {
                    result.Add(new Image { Name = filteredName, Id = null });
                }
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("sync")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SyncImagesAsync()
        {
            var newList = await _imagesService.SyncImagesAsync();
            return Ok(newList);
        }
    }
}
