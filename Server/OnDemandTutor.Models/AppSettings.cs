using System.Text.Json.Serialization;

namespace OnDemandTutor.Models;

public partial class AppSetting
{
    [JsonPropertyName("Logging")]
    public Logging Logging { get; set; } = default!;
    [JsonPropertyName("SmtpSettings")]
    public SmtpAppSetting SmtpAppSetting { get; set; } = default!;
    [JsonPropertyName("VnPay")]
    public VnPay VnPay { get; set; } = default!;
}

public partial class Logging
{
    [JsonPropertyName("LogLevel")]
    public LogLevel LogLevel { get; set; } = default!;
}

public partial class LogLevel
{
    [JsonPropertyName("Default")]
    public string Default { get; set; } = string.Empty;

    [JsonPropertyName("Microsoft.AspNetCore")]
    public string MicrosoftAspNetCore { get; set; } = string.Empty;
}

public class SmtpAppSetting
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SmtpUserName { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
    public string AppVerify { get; set; } = string.Empty;
    // public string SmtpFromAddress { get; set; }
}

public class VnPay
{
    public string TmnCode { get; set; } = string.Empty;
    public string HashSecret { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string CurrCode { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}