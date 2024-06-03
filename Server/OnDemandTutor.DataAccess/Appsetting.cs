using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnDemandTutor.DataAccess
{
    public partial class AppSetting
    {
        [JsonPropertyName("Logging")]
        public Logging Logging { get; set; }

         [JsonPropertyName("ConnectionStrings")]
        public ConnectionStrings ConnectionStrings { get; set; }

        [JsonPropertyName("JWTSettings")]
        public JwtSettings JwtSettings { get; set; }
        [JsonPropertyName("AllowedHosts")]
        public string AllowedHosts { get; set; }
        public bool ShowInternalServerError { get; set; }

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

       public partial class ConnectionStrings
    {
        [JsonPropertyName("DbName")]
        public string DbName { get; set; }
    }

     public partial class JwtSettings
    {
        [JsonPropertyName("securityKey")]
        public string SecurityKey { get; set; }

        [JsonPropertyName("validIssuer")]
        public string ValidIssuer { get; set; }

        [JsonPropertyName("validAudience")]
        public string ValidAudience { get; set; }

        [JsonPropertyName("expiryInMinutes")]
        public long ExpiryInMinutes { get; set; }
    }
}
