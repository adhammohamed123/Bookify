using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Apartment;
using Bookify.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking.ReserveBooking
{
    public record ReserveBookingCommand(
        Guid UserId,
        Guid ApartmentId,
        DateTime StartDateUtc,
        DateTime EndDateUtc) : ICommand<Guid>;

}
