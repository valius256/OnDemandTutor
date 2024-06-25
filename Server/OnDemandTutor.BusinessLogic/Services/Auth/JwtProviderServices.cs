using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Enum;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace OnDemandTutor.BusinessLogic.Services.Auth;

public class JwtProviderServices : IJwtProviderServices
{
    private readonly IFireBaseAuthServices _fireBaseAuthServices;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IUserServices _userServices;

    public JwtProviderServices(HttpClient httpClient, IUnitOfWorkRepository unitOfWorkRepository,
        IUserServices userServices, IFireBaseAuthServices fireBaseAuthServices)
    {
        _httpClient = httpClient;
        _unitOfWorkRepository = unitOfWorkRepository;
        _userServices = userServices;
        _fireBaseAuthServices = fireBaseAuthServices;
    }

    public async Task<string> GetForCredentialsAsync(string email, string password)
    {
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
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Invalid credentials provided");
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new HttpRequestException($"Bad request: {errorContent}");
                throw new HttpRequestException(
                    $"Request failed with status code {response.StatusCode}: {errorContent}");
            }

            var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();
            var userInDb = _unitOfWorkRepository.UserRepository.FirstOrDefault(x => x.FireBaseid == authToken.LocalId);
            if (userInDb == null)
            {
                _unitOfWorkRepository.UserRepository.Add(new Models.Models.User
                {
                    Email = authToken.Email,
                    Password = password,
                    FireBaseid = authToken.LocalId,
                    FirstName = authToken.DisplayName,
                    Role = RoleStatus.Customer,
                    Sex = Sex.Male
                });
                await _unitOfWorkRepository.SaveChangesAsync();
            }

            if (authToken == null) throw new InvalidOperationException("Authentication token is null");

            var customClaims = new Dictionary<string, object>
            {
                { "role", userInDb.Role.ToString() },
                { "id", userInDb.Id.ToString() }
            };

            await _fireBaseAuthServices.SetCustomClaimsAsync(authToken.LocalId, customClaims);
            return authToken.IdToken;
        }
        catch (Exception exception)
        {
            return exception.Message;
        }
    }

    public class AuthToken
    {
        [JsonPropertyName("kind")] public string Kind { get; set; }

        [JsonPropertyName("localId")] public string LocalId { get; set; }

        [JsonPropertyName("email")] public string Email { get; set; }

        [JsonPropertyName("displayName")] public string DisplayName { get; set; }

        [JsonPropertyName("idToken")] public string IdToken { get; set; }

        [JsonPropertyName("registered")] public bool Registered { get; set; }

        [JsonPropertyName("refreshToken")] public string RefreshToken { get; set; }

        [JsonPropertyName("expiresIn")] public long ExpiresIn { get; set; }

        [JsonPropertyName("role")] public string Role { get; set; }
    }
}