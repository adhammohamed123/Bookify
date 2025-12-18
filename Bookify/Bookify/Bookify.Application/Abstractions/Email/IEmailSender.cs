
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Abstractions.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Bookify.Domain.User.Email email , string subject, string body, CancellationToken cancellationToken = default);
    }
}
