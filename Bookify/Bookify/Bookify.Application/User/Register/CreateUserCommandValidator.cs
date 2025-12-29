using FluentValidation;

namespace Bookify.Application.User.Register
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            // Add Rules
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c=>c.FirestName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(100);
            RuleFor(c=> c.Email).EmailAddress().NotEmpty().MaximumLength(100);
        }
    }
}
