using System.Security.Claims;
using OnDemandTutor.Models.Dtos.Upload;

namespace OnDemandTutor.BusinessLogic.Interfaces.Upload;

public interface IFirebaseUploadServices
{
    Task<string> UploadImageAsync(ClaimsPrincipal claimsPrincipal, string fileName, Stream fileStream);
    Task<List<DowloadImagesDtos>> DownloadImagesAsync(string uid);
}