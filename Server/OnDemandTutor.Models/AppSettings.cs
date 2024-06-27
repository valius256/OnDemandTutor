using System.Text.Json.Serialization;

namespace OnDemandTutor.Models;

public partial class AppSetting
{
    [JsonPropertyName("Logging")]
    public Logging Logging { get; set; }
    [JsonPropertyName("SmtpSettings")]
    public SmtpAppSetting SmtpAppSetting { get; set; }
    [JsonPropertyName("VnPay")]
    public VnPay VnPay { get; set; }
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

public class VnPay
{
    public string TmnCode {get; set; }  
    public string HashSecret { get; set; }
    public string BaseUrl { get; set; }
    public string Command { get; set; } 
    public string CurrCode { get; set; }
    public string Locale { get; set; } 
    public string Version { get; set; } 
}