using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Review
{
    public class ReviewErrors
    {
        public static Error BookingNotCompleted => new(
            "Review.BookingNotCompleted",
            "Cannot create a review for a booking that is not completed.");
    }
}
