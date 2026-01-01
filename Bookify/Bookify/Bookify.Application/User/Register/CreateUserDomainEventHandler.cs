using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.User;
using Bookify.Domain.User.Events;
using MediatR;

namespace Bookify.Application.User.Register
{
    internal sealed class CreateUserDomainEventHandler(
      IEmailSender emailSender,
      IRepositoryManager repositoryManager

           ) : INotificationHandler<UserCreatedDomainEvent>
    {
        private readonly IEmailSender emailSender = emailSender;
        private readonly IRepositoryManager repositoryManager = repositoryManager;

        public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await repositoryManager.UserRepository.GetUserAsync(notification.userId, false);
            if (user is null) return; 

            await emailSender.SendEmailAsync(
                Email.Create("adhammo909@gmail.com"),
                "New User Is Registerd In Our Bookify System",
                $"this user with id {notification.userId} and {user.FirstName.Value}_{user.LastName.Value} is now created as a new user",
                false, cancellationToken);

            await emailSender.SendEmailAsync(
               Email.Create(notification.email),
               "Welcome to Bookify System",
               $"Hello {user.FirstName.Value} , We are plased to Your Registraion in our Bookify System",
               false, cancellationToken);
        }
    }
}
