using Bookify.Application.Abstractions.Email;
using Bookify.Domain.User;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Bookify.Infrastracture.Email
{
    public class EmailSettings
    {
        public required string SmtpServer { get; set; }
        public required int Port { get; set; }
        public required string From { get; set; }
        public required string DisplayName { get; set; }
        public required string ReplayTo { get; set; }
        public required string Password { get; set; }
        public bool EnableSsl { get; set; }


    }



    internal sealed class EmailSender(IOptions<EmailSettings> settingsOptions) : IEmailSender
    {
        private readonly EmailSettings settings = settingsOptions.Value;

        public async Task SendEmailAsync(Bookify.Domain.User.Email email, string subject, string body,bool isHtml, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(settings.DisplayName,settings.From));
            message.To.Add(MailboxAddress.Parse(email.Value));
            message.ReplyTo.Add(MailboxAddress.Parse(settings.ReplayTo));
            message.Subject = subject;

            message.Body = new TextPart(isHtml?"html" :"plain")
            {
                Text= body 
            };

           using var smtpserver = new SmtpClient();
           await smtpserver.ConnectAsync(settings.SmtpServer,settings.Port,settings.EnableSsl == true? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None ,cancellationToken);

            await smtpserver.AuthenticateAsync(settings.From, settings.Password, cancellationToken);
            
            await smtpserver.SendAsync(message,cancellationToken);
            
        }
    }
}
