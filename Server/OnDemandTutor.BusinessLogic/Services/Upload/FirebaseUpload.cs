using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using OnDemandTutor.BusinessLogic.Interfaces.Upload;
using OnDemandTutor.Models.Dtos.Upload;

namespace OnDemandTutor.BusinessLogic.Services.Upload
{
    public class FirebaseUploadServices : IFirebaseUploadServices
    {
        private readonly string StorageBucketName = "ondemandtutor-a049e.appspot.com";

        private readonly string ServiceAccountPath =
            @"D:\Semester7\SWD392\OnDemandTutor\Server\OnDemandTutor.API\firebase.json";


        public async Task<string> UploadImageAsync(string uid, string fileName, Stream fileStream)
        {
            try
            {
                string imageUrl = await UploadImageToFirebaseStorage(uid, fileName, fileStream);
                return imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload image: {ex.Message}");
                throw;
            }
        }

        private async Task<string> UploadImageToFirebaseStorage(string uid, string fileName, Stream fileStream)
        {
            try
            {
                var storage = StorageClient.Create();
                string storageFileName = $"images/{uid}/{fileName}";

                // Upload image to Firebase Storage
                await storage.UploadObjectAsync(
                    bucket: StorageBucketName,
                    objectName: storageFileName,
                    contentType: "image/jpeg",
                    source: fileStream
                );

                string imageUrl = $"https://storage.googleapis.com/{StorageBucketName}/{storageFileName}";
                Console.WriteLine($"Image uploaded to Firebase Storage: {imageUrl}");
                return imageUrl;
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

        public async Task<List<DowloadImagesDtos>> DownloadImagesAsync(string uid)
        {
            List<DowloadImagesDtos> downloadLinksList = new List<DowloadImagesDtos>();
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
                    string fileName = obj.Name.Substring(obj.Name.LastIndexOf('/') + 1);

                    // Generate a signed URL for each image
                    string url = urlSigner.Sign(
                        bucket: obj.Bucket,
                        objectName: obj.Name,
                        duration: TimeSpan.FromHours(1),
                        HttpMethod.Get);

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
    }


}
