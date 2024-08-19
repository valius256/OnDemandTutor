using OnDemandTutor.Models.Dtos.Upload;
using OnDemandTutor.Models.Dtos.User;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.Upload;

public interface IFirebaseUploadServices
{
    Task<string> UploadImageAsync(GetProfileUserDto user, string fileName, Stream fileStream);
    Task<List<DowloadImagesDtos>> DownloadImagesAsync(string uid);
    Task<string> UploadVideoAsync(GetProfileUserDto user, string fileName, Stream fileStream);
}