using OnDemandTutor.Models.Dtos.Upload;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.Upload;

public interface IFirebaseUploadServices
{
    Task<string> UploadImageAsync(ClaimsPrincipal claimsPrincipal, string fileName, Stream fileStream);
    Task<List<DowloadImagesDtos>> DownloadImagesAsync(string uid);
    Task<string> UploadVideoAsync(ClaimsPrincipal claimsPrincipal, string fileName, Stream fileStream);
}