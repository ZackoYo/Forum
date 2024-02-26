using Forum.Domain.Infrastructure.ExternalServices;

namespace Forum.Infrastructure.Contracts.ExternalServices;

public interface IEmailSender
{
	Task SendEmailAsync(
		string from,
		string fromName,
		string to,
		string subject,
		string htmlContent,
		IEnumerable<EmailAttachment> attachments = null);
}