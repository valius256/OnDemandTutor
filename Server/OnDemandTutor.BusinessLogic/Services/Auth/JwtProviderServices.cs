using FirebaseAdmin.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Dtos.Authen;
using OnDemandTutor.Models.Enum;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace OnDemandTutor.BusinessLogic.Services.Auth;

public class JwtProviderServices : IJwtProviderServices
{
    private readonly IFireBaseAuthServices _fireBaseAuthServices;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;

    public JwtProviderServices(HttpClient httpClient, IUnitOfWorkRepository unitOfWorkRepository,
         IFireBaseAuthServices fireBaseAuthServices)
    {
        _httpClient = httpClient;
        _unitOfWorkRepository = unitOfWorkRepository;
        _fireBaseAuthServices = fireBaseAuthServices;
    }

 public async Task<AuthenResponseDto> GetForCredentialsAsync(string email, string password)
{
    var responseModel = new AuthenResponseDto();

    try
    {
        var request = new
        {
            email,
            password,
            returnSecureToken = true
        };

        var response = await _httpClient.PostAsJsonAsync("", request);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();

            try
            {
                // Parse the error content as JSON
                var parsedJson = Newtonsoft.Json.Linq.JObject.Parse(errorContent);
                responseModel.code = parsedJson["error"]?["code"]?.ToString() ?? throw new InvalidOperationException();
                responseModel.message = parsedJson["error"]?["message"]?.ToString() ?? throw new InvalidOperationException();
            }
            catch (Exception)
            {
                // If parsing fails, set code to the status code and use raw content as message
                responseModel.code = response.StatusCode.ToString();
                responseModel.message = errorContent;
            }

            return responseModel;
        }

        var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();
        if (authToken == null)
            throw new InvalidOperationException("Authentication token is null");

        var userInDb = _unitOfWorkRepository.UserRepository.FirstOrDefault(x => x.FireBaseid == authToken.LocalId);
        if (userInDb == null)
        {
            userInDb = new Models.Models.User
            {
                Email = authToken.Email,
                Password = password,
                FireBaseid = authToken.LocalId,
                FirstName = authToken.DisplayName,
                Role = RoleStatus.Customer,
                Sex = Sex.Male
            };
            _unitOfWorkRepository.UserRepository.Add(userInDb);
            await _unitOfWorkRepository.SaveChangesAsync();
        }

        var customClaims = new Dictionary<string, object>
        {
            { "roles", userInDb.Role.ToString() },
            { "id", userInDb.Id.ToString() }
        };
        await _fireBaseAuthServices.SetCustomClaimsAsync(authToken.LocalId, customClaims);

        // Reauthenticate the user to get a new token with the custom claims
        var reAuthRequest = new
        {
            email,
            password,
            returnSecureToken = true
        };

        var reAuthResponse = await _httpClient.PostAsJsonAsync("", reAuthRequest);
        if (!reAuthResponse.IsSuccessStatusCode)
        {
            responseModel.code = reAuthResponse.StatusCode.ToString();
            responseModel.message = await reAuthResponse.Content.ReadAsStringAsync();
            return responseModel;
        }

        var reAuthToken = await reAuthResponse.Content.ReadFromJsonAsync<AuthToken>();
        if (reAuthToken == null)
            throw new InvalidOperationException("Re-authentication token is null");

        var options = new SessionCookieOptions
        {
            ExpiresIn = TimeSpan.FromDays(7),
        };

        var cookieExtendSession = await _fireBaseAuthServices.CreateSessionCookieAsync(reAuthToken.IdToken, options);
        responseModel.code = response.StatusCode.ToString();
        responseModel.message = cookieExtendSession;
    }
    catch (Exception exception)
    {
        responseModel.code = "Error";
        responseModel.message = exception.Message;
    }

    return responseModel;
}



    public class AuthToken
    {
        [JsonPropertyName("kind")] public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("localId")] public string LocalId { get; set; } = string.Empty;

        [JsonPropertyName("Email")] public string Email { get; set; } = string.Empty;

        [JsonPropertyName("displayName")] public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("idToken")] public string IdToken { get; set; } = string.Empty;

        [JsonPropertyName("registered")] public bool Registered { get; set; }

        [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("expiresIn")] public long ExpiresIn { get; set; }

        [JsonPropertyName("role")] public string Role { get; set; } = string.Empty;
    }
}