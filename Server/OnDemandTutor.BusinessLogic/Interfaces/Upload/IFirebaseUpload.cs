using OnDemandTutor.Models.Dtos.Upload;

namespace OnDemandTutor.BusinessLogic.Interfaces.Upload;

public interface IFirebaseUploadServices
{
    Task<string> UploadImageAsync(string uid, string fileName, Stream fileStream);
    Task<List<DowloadImagesDtos>> DownloadImagesAsync(string uid);
}