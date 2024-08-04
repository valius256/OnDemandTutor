using OnDemandTutor.Models.Dtos.Upload;
using OnDemandTutor.Models.Dtos.User;

namespace OnDemandTutor.BusinessLogic.Interfaces.Upload;

public interface IFirebaseUploadServices
{
    Task<string> UploadImageAsync(GetProfileUserDtos user, string fileName, Stream fileStream);
    Task<List<DowloadImagesDtos>> DownloadImagesAsync(string uid);
    Task<string> UploadVideoAsync(GetProfileUserDtos user, string fileName, Stream fileStream);
}