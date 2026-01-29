using Bookify.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking.ConfirmBooking
{

    public record ConfirmBookingCommand( Guid BookingId) : ICommand;

}
