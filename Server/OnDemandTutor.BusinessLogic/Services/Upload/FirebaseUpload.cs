using System.Security.Claims;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using OnDemandTutor.BusinessLogic.Interfaces.Upload;
using OnDemandTutor.Models.Dtos.Upload;

namespace OnDemandTutor.BusinessLogic.Services.Upload;

public class FirebaseUploadServices : IFirebaseUploadServices
{
    private readonly string ServiceAccountPath =
        @"D:\Semester7\SWD392\OnDemandTutor\Server\OnDemandTutor.API\firebase.json";

    private readonly string StorageBucketName = "ondemandtutor-a049e.appspot.com";


    public async Task<string> UploadImageAsync(ClaimsPrincipal claimsPrincipal, string fileName, Stream fileStream)
    {
        try
        {
            var userUid = claimsPrincipal.FindFirst(c => c.Type == "user_id")?.Value;
            var imageUrl = await UploadImageToFirebaseStorage(userUid, fileName, fileStream);
            return imageUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to upload image: {ex.Message}");
            throw;
        }
    }


    public async Task<List<DowloadImagesDtos>> DownloadImagesAsync(string uid)
    {
        var downloadLinksList = new List<DowloadImagesDtos>();
        var storage = StorageClient.Create(GoogleCredential.FromFile(ServiceAccountPath));
        try
        {
            // Reference to the folder containing images for the given uid
            var objects = storage.ListObjects(StorageBucketName, $"images/{uid}/");

            var urlSigner = UrlSigner.FromServiceAccountCredential(
                GoogleCredential.FromFile(ServiceAccountPath).UnderlyingCredential as ServiceAccountCredential);


            foreach (var obj in objects)
            {
                // Extracting the file name from the full object name
                var fileName = obj.Name.Substring(obj.Name.LastIndexOf('/') + 1);

                // Generate a signed URL for each image
                var url = urlSigner.Sign(
                    obj.Bucket,
                    obj.Name,
                    TimeSpan.FromDays(7),
                    HttpMethod.Get);
                var fetchUrl = storage.GetObjectAsync(obj.Bucket, obj.Name);

                // Create DTO object
                var dto = new DowloadImagesDtos
                {
                    Uid = uid,
                    Url = url,
                    FileName = fileName // Assigning the extracted file name
                };

                downloadLinksList.Add(dto);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating download links: {ex.Message}");
            // Handle exceptions according to your requirement
        }

        return downloadLinksList;
    }

    private async Task EnsurePublicAccess(StorageClient storage)
    {
        var policy = storage.GetBucketIamPolicy(StorageBucketName);
        if (!policy.Bindings.Any(b => b.Role == "roles/storage.objectViewer" && b.Members.Contains("allUsers")))
        {
            policy.Bindings.Add(new Policy.BindingsData
            {
                Role = "roles/storage.objectViewer",
                Members = new List<string> { "allUsers" }
            });
            storage.SetBucketIamPolicy(StorageBucketName, policy);
        }
    }

    private async Task<string> UploadImageToFirebaseStorage(string uid, string fileName, Stream fileStream)
    {
        try
        {
            var storage = StorageClient.Create();
            await EnsurePublicAccess(storage);

            var storageFileName = $"images/{uid}/{fileName}";
            await storage.UploadObjectAsync(StorageBucketName, storageFileName, "image/jpeg", fileStream);

            var storageObject = await storage.GetObjectAsync(StorageBucketName, storageFileName);
            storage.UpdateObject(storageObject,
                new UpdateObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead });

            return $"https://storage.googleapis.com/{StorageBucketName}/{storageFileName}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to upload image to Firebase Storage: {ex.Message}");
            throw;
        }
        finally
        {
            // Dispose the stream to release resources
            fileStream.Dispose();
        }
    }
}