using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Review
{
    public record Rating
    {
        public static Error InValid=> new("Rating.Invalid", "Rating must be between 1 and 5.");

        public int Value { get;}
        private Rating(int value)
        {
            Value = value;
        }
        public static Result<Rating> Create(int value)
        {
            if (value < 1 || value > 5)
            {
                return Result.Failure<Rating>(InValid);
            }
            return Result.Success(new Rating(value));
        }

    }
}
