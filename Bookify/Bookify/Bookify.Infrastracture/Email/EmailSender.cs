using Bookify.Application.Abstractions.Email;
using Bookify.Domain.User;

namespace Bookify.Infrastracture.Email
{
    internal sealed class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(Bookify.Domain.User.Email email, string subject, string body, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"To:{email.Value}");
            Console.WriteLine($"Subject:{subject}");
            Console.WriteLine($"Body:{body}");
            return Task.CompletedTask;
        }
    }
}
