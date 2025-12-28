using Bookify.Domain.Booking;
using Bookify.Domain.Shared;

namespace Bookify.Application.Booking.Dtos
{
   public record BookingForCreationDto
   {
        public Guid UserId { get; set; }
        public Guid ApartmentId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
    public record BookingDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get;  set; }
        public Guid ApartmentId { get;  set; }
       
        public required DateRange Duration { get;  set; }
        public required Money PricePerPeriod { get;  set; }
        public required Money CleanningFee { get;  set; }
        public required Money AmenitiesUpCharge { get;  set; }
        public required Money TotalPrice { get;  set; }

        public BookingStatus Status { get;  set; }

        public DateTime CreatedAtUtc { get;  set; }
        public DateTime? ConfirmedAtUtc { get;  set; }
        public DateTime? RejectedAtUtc { get;  set; }
        public DateTime? CompletedAtUtc { get;  set; }
        public DateTime? CanceledAtUtc { get;  set; }
    }
}
