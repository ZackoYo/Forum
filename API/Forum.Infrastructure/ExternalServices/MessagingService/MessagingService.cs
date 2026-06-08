namespace Forum.Infrastructure.ExternalServices.MessagingService;

public class EmailSettings
{
    public const string SectionName = "Email";

    public string Provider { get; set; } = EmailProviderNames.Null;

    public SendGridSettings SendGrid { get; set; } = new();
}

public static class EmailProviderNames
{
    public const string Null = "Null";

    public const string SendGrid = "SendGrid";
}

public class SendGridSettings
{
    public string? ApiKey { get; set; }

    public string? DefaultFromEmail { get; set; }

    public string? DefaultFromName { get; set; }
}
