using FluentValidation;

namespace Bookify.Application.Review.CreateReviewCommand
{
    internal sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.ApartmentId).NotEmpty().WithMessage("ApartmentId is required.");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.BookingId).NotEmpty().WithMessage("BookingId is required.");
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");
            RuleFor(x => x.Comment).MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
        }
    }
}
