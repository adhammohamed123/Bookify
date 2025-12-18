using Bookify.Domain.Booking;
using Bookify.Domain.Shared;

namespace Bookify.Application.Booking.Dtos
{

    public record BookingForCreationDto
    {
        public Guid UserId { get; set; }
        public Guid ApartmentId { get; set; }
        public DateTime Duration_Start { get; set; }
        public DateTime Duration_End{ get; set; }

    }

    public record BookingForUpdateDto:BookingForCreationDto
    {
        public BookingStatus Status { get; set; }
    }

    public record BookingDto:BookingForUpdateDto
    {

        public Guid Id { get; set; }
        public required Money PricePerPeriod { get;  set; }
        public required Money CleanningFee { get;  set; }
        public required Money AmenitiesUpCharge { get;  set; }
        public required Money TotalPrice { get;  set; }
        public DateTime CreatedAtUtc { get;  set; }
        public DateTime? ConfirmedAtUtc { get;  set; }
        public DateTime? RejectedAtUtc { get;  set; }
        public DateTime? CompletedAtUtc { get;  set; }
        public DateTime? CanceledAtUtc { get;  set; }
    }
}
