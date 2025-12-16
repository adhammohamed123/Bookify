using Bookify.Domain.Abstractions;

namespace Bookify.Domain.User
{
    public record Email
    {
        public static Error InValid =>new Error("Email.Invalid", "Email is not valid.");
        private Email(string value)
        {
            Value = value;
        }
        public string Value { get; private set; }
        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                return Result<Email>.Failure(InValid);
            }
            return Result<Email>.Success(new Email(email));
        }

    }


}
