using System.Text.Json.Serialization;

namespace OnDemandTutor.Models
{
    public partial class AppSetting
    {
        [JsonPropertyName("Logging")]
        public Logging Logging { get; set; }

        [JsonPropertyName("ConnectionStrings")]
        public ConnectionStrings ConnectionStrings { get; set; }

        [JsonPropertyName("GoogleAuthSettings")]
        public GoogleAuthSettings GoogleAuthSettings { get; set; }

        [JsonPropertyName("JWTSettings")]
        public JwtSettings JwtSettings { get; set; }

        [JsonPropertyName("AllowedHosts")]
        public string AllowedHosts { get; set; }

        [JsonPropertyName("DefaultCountry")]
        public string DefaultCountry { get; set; }

        [JsonPropertyName("UploadSettings")]
        public UploadSettings UploadSettings { get; set; }
        public SmtpAppSetting SmtpAppSetting { get; set; }
        [JsonPropertyName("GoogleAPISetting")]
        public GoogleAPISetting GoogleAPISetting { get; set; }
        [JsonPropertyName("HostName")]
        public string HostName { get; set; }
        [JsonPropertyName("UploadRequestPath")]
        public string UploadRequestPath { get; set; }
        public string languages { get; set; }
        public bool ShowInternalServerError { get; set; }

    }

    public partial class ConnectionStrings
    {
        [JsonPropertyName("DbName")]
        public string DbName { get; set; }
    }

    public partial class GoogleAuthSettings
    {
        [JsonPropertyName("clientIds")]
        public List<string> ClientIds { get; set; }
    }

    public partial class JwtSettings
    {
        [JsonPropertyName("securityKey")]
        public string SecurityKey { get; set; }

        [JsonPropertyName("validIssuer")]
        public string ValidIssuer { get; set; }

        [JsonPropertyName("expiryInMinutes")]
        public long ExpiryInMinutes { get; set; }
    }

    public partial class Logging
    {
        [JsonPropertyName("LogLevel")]
        public LogLevel LogLevel { get; set; }
    }

    public partial class LogLevel
    {
        [JsonPropertyName("Default")]
        public string Default { get; set; }

        [JsonPropertyName("Microsoft.AspNetCore")]
        public string MicrosoftAspNetCore { get; set; }
    }

    public partial class UploadSettings
    {
        [JsonPropertyName("UploadProvider")]
        public string UploadProvider { get; set; }

        [JsonPropertyName("Directory")]
        public string Directory { get; set; }

        [JsonPropertyName("MaxFileSize")]
        public int MaxFileSize { get; set; }

        [JsonPropertyName("GoogleDriveCredential")]
        public GoogleDriveCredential GoogleDriveCredential { get; set; }
    }

    public partial class GoogleDriveCredential
    {
        [JsonPropertyName("ClientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("ClientSecret")]
        public string ClientSecret { get; set; }
    }

    public partial class GoogleAPISetting
    {
        [JsonPropertyName("ProjectId")]
        public string ProjectId { get; set; }

        [JsonPropertyName("ClientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("ClientSecret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("RedirectUrlSystem")]
        public string RedirectUrlSystem { get; set; }

        [JsonPropertyName("Hd")]
        public string Hd { get; set; }

        [JsonPropertyName("CredentialPath")]
        public string CredentialPath { get; set; }
    }


    public class SmtpAppSetting
    {
        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public bool SmtpUseSSL { get; set; }

        public string SmtpUserName { get; set; }

        public string SmtpPassword { get; set; }

        public string SmtpFromAddress { get; set; }
    }

}
