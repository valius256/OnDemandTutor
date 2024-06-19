using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnDemandTutor.BusinessLogic.Interfaces.Upload;
namespace OnDemandTutor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IFirebaseUploadServices _firebaseUploadServices;

        public UploadController(IFirebaseUploadServices firebaseUploadServices)
        {
            _firebaseUploadServices = firebaseUploadServices;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(string uid, string fileName, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("File is empty");
    
                // Upload image to Firebase Storage and get the URL
                using (var stream = file.OpenReadStream())
                {
                    string imageUrl = await _firebaseUploadServices.UploadImageAsync(uid, fileName, stream);
                    return Ok(imageUrl);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to upload file: {ex.Message}");
            }
        }
        
        
        [HttpPost("get-image-list")]
        public async Task<IActionResult> Load(string fireBaseId)
        { 
            try
            {
                // Get the URL from Firebase Realtime Database
                var imageUrl = await _firebaseUploadServices.DownloadImagesAsync(fireBaseId);

                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to load file: {ex.Message}");
            }
        }

    }

}