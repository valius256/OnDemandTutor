using System.Text.Json.Serialization;

namespace OnDemandTutor.Models;

public partial class AppSetting
{
    [JsonPropertyName("Logging")]
    public Logging Logging { get; set; }
    [JsonPropertyName("SmtpSettings")]
    public SmtpAppSetting SmtpAppSetting { get; set; }
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

public class SmtpAppSetting
{
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUserName { get; set; }
    public string SmtpPassword { get; set; }
    public bool EnableSsl { get; set; } 
    public string AppVerify { get; set; }
    // public string SmtpFromAddress { get; set; }
}