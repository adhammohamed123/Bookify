namespace Bookify.Application.Review.Dtos
{
    public record ReviewForCreationDto
    {
        public Guid ApartmentId { get; init; }
        public Guid UserId { get; init; }
        public Guid BookingId { get; init; }
        public int Rating { get; init; }
        public string? Comment { get; init; }
    }

    public record ReviewDto: ReviewForCreationDto
    {
        public Guid Id { get; init; }
        public DateTime CreatedAtUtc { get; init; }
    }
}
