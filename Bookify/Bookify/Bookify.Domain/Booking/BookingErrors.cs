using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Booking
{
    public static class BookingErrors
    {
        public static Error BookingNotFound => new("Booking.NotFound", "The specified booking was not found.");
        public static Error FailedProcess=> new("Booking.FailedProcess", "Failed to process the booking.");

        public static Error NotReserved => new("Booking.NotReserved", "this booking is no longer be pending");

        public static Error NotConfirmed => new("Booking.NotConfirmed", "this booking is not Confirmed at this time");
        public static Error BookingDateOverLapped => new("Booking.DateOverLapped", "The booking dates overlap with an existing booking.");
    }

}
