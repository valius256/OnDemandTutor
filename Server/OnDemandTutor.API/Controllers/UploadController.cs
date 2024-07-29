using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Upload;

namespace OnDemandTutor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IFirebaseUploadServices _firebaseUploadServices;
    private readonly IAuthServices _authServices;
    public UploadController(IFirebaseUploadServices firebaseUploadServices, IAuthServices authServices)
    {
        _firebaseUploadServices = firebaseUploadServices;
        _authServices = authServices;
    }

    [HttpPost("upload-image")]
    [Authorize]
    public async Task<IActionResult> Upload(string fileName, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty");

        // Upload image to Firebase Storage and get the URL
        using (var stream = file.OpenReadStream())
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var imageUrl = await _firebaseUploadServices.UploadImageAsync(user, fileName, stream);
            return Ok(imageUrl);
        }
    }


    [HttpPost("get-image-list")]
    [Authorize]
    public async Task<IActionResult> Load(string fireBaseId)
    {
        // Get the URL from Firebase Storage Database
        var imageUrl = await _firebaseUploadServices.DownloadImagesAsync(fireBaseId);
        return Ok(imageUrl);
    }

    // accept 25 mb file
    [HttpPost("upload-video")]
    [Authorize]
    public async Task<IActionResult> UploadVideo(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty");

        using (var stream = file.OpenReadStream())
        {
            var user = await _authServices.GetUserProfileByClaim(HttpContext.User);
            var videoUrl = await _firebaseUploadServices.UploadVideoAsync(user, file.FileName, stream);
            return Ok(videoUrl);
        }
    }


}