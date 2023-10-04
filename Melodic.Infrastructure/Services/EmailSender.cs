using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Melodic.Infrastructure.Services;

public class AuthMessageSenderOption
{
    public static string AuthMessagesSender = "AuthMessageSenderOption";
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Host { get; set; }
}

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOption> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOption Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (Options == null)
            throw new Exception("Null Auth Message Sender Options");

        await Execute(Options, subject, message, toEmail);
    }

    public async Task Execute(AuthMessageSenderOption options, string subject, string message, string toEmail)
    {
        var client = new SmtpClient(options.Host, 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(options.Email, options.Password)
        };
        MailMessage mailMessage = new MailMessage(from: options.Email!,
                          to: toEmail,
                          subject,
                          message
                          );
        mailMessage.IsBodyHtml = true;
        await client.SendMailAsync(mailMessage);
        _logger.LogInformation(mailMessage.DeliveryNotificationOptions == DeliveryNotificationOptions.OnSuccess
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
    }
}
