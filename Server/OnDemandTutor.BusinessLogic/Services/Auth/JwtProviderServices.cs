using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace OnDemandTutor.BusinessLogic.Services.Auth
{
    public class JwtProviderServices : IJwtProviderServices
    {
        private readonly HttpClient _httpClient;
        public JwtProviderServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetForCredentialsAsync(string email, string password)
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
                {
                    throw new UnauthorizedAccessException("Invalid credentials provided");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException($"Bad request: {errorContent}");
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
                }
            }

            var authToken = await response.Content.ReadFromJsonAsync<AuthToken>();

            if (authToken == null)
            {
                throw new InvalidOperationException("Authentication token is null");
            }

            return authToken.IdToken;
        }

        public class AuthToken
        {
            [JsonPropertyName("kind")]
            public string Kind { get; set; }
            [JsonPropertyName("localId")]
            public string LocalId { get; set; }
            [JsonPropertyName("email")]
            public string Email { get; set; }
            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; }
            [JsonPropertyName("idToken")]
            public string IdToken { get; set; }
            [JsonPropertyName("registered")]
            public bool Registered { get; set; }
            [JsonPropertyName("refreshToken")]
            public string RefreshToken { get; set; }
            [JsonPropertyName("expiresIn")]
            public long ExpiresIn { get; set; }
        }
    }
}
