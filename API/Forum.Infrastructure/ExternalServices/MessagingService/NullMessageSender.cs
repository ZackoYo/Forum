using Forum.Domain.Infrastructure.ExternalServices;
using Forum.Infrastructure.Contracts.ExternalServices;
using Microsoft.Extensions.Logging;

namespace Forum.Infrastructure.ExternalServices.MessagingService;

public class NullMessageSender : IEmailSender
{
    private readonly ILogger<NullMessageSender> logger;

    public NullMessageSender(ILogger<NullMessageSender> logger)
    {
        this.logger = logger;
    }

    public Task SendEmailAsync(
        string from,
        string fromName,
        string to,
        string subject,
        string htmlContent,
        IEnumerable<EmailAttachment>? attachments = null)
    {
        this.logger.LogInformation(
            "Email sending is disabled. Skipping email to {Recipient} with subject {Subject}.",
            to,
            subject);

        return Task.CompletedTask;
    }
}
