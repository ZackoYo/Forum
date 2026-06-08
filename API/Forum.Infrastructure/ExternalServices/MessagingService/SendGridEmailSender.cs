using Forum.Domain.Infrastructure.ExternalServices;
using Forum.Infrastructure.Contracts.ExternalServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Forum.Infrastructure.ExternalServices.MessagingService;

public class SendGridEmailSender : IEmailSender
{
    private readonly SendGridClient client;
    private readonly SendGridSettings settings;
    private readonly ILogger<SendGridEmailSender> logger;

    public SendGridEmailSender(IOptions<EmailSettings> options, ILogger<SendGridEmailSender> logger)
    {
        this.settings = options.Value.SendGrid;
        this.logger = logger;

        if (string.IsNullOrWhiteSpace(this.settings.ApiKey))
        {
            throw new InvalidOperationException("SendGrid is selected as the email provider, but Email:SendGrid:ApiKey is not configured.");
        }

        this.client = new SendGridClient(this.settings.ApiKey);
    }

    public async Task SendEmailAsync(
        string from,
        string fromName,
        string to,
        string subject,
        string htmlContent,
        IEnumerable<EmailAttachment>? attachments = null)
    {
        if (string.IsNullOrWhiteSpace(to))
        {
            throw new ArgumentException("Recipient email should be provided.", nameof(to));
        }

        if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
        {
            throw new ArgumentException("Subject or HTML content should be provided.");
        }

        var senderEmail = string.IsNullOrWhiteSpace(from) ? this.settings.DefaultFromEmail : from;
        var senderName = string.IsNullOrWhiteSpace(fromName) ? this.settings.DefaultFromName : fromName;

        if (string.IsNullOrWhiteSpace(senderEmail))
        {
            throw new ArgumentException("Sender email should be provided either by the caller or by Email:SendGrid:DefaultFromEmail.", nameof(from));
        }

        var fromAddress = new EmailAddress(senderEmail, senderName);
        var toAddress = new EmailAddress(to);
        var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);

        if (attachments?.Any() == true)
        {
            foreach (var attachment in attachments)
            {
                message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
            }
        }

        Response response;
        try
        {
            response = await this.client.SendEmailAsync(message);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "Unexpected error while sending email to {Recipient} with SendGrid.", to);
            throw;
        }

        var responseBody = response.Body == null ? string.Empty : await response.Body.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            this.logger.LogError(
                "SendGrid failed to send email to {Recipient}. StatusCode: {StatusCode}. ResponseBody: {ResponseBody}",
                to,
                response.StatusCode,
                responseBody);

            throw new InvalidOperationException($"SendGrid failed to send email. Status code: {response.StatusCode}.");
        }

        this.logger.LogInformation("SendGrid accepted email to {Recipient}. StatusCode: {StatusCode}", to, response.StatusCode);
    }
}
