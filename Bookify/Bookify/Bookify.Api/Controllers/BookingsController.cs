using Bookify.Application.Booking.Dtos;
using Bookify.Application.Booking.GetBooking;
using Bookify.Application.Booking.ReserveBooking;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers
{
    [ApiController]
    [Route("api/Bookings")]
    public class BookingsController(ISender sender) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetBookingById(Guid id,CancellationToken cancellationToken)
        {
            var bookingQuery = new GetBookingQurey(id);
            var result=  await sender.Send(bookingQuery, cancellationToken);
           return  result.IsSuccess?  Ok(result.Value): NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ReserveBooking(BookingForCreationDto dto,CancellationToken cancellationToken)
        {
            var reservationCommand = new ReserveBookingCommand(dto.UserId, dto.ApartmentId, dto.Start, dto.End);
            var result = await sender.Send(reservationCommand, cancellationToken);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetBookingById), new { id= result.Value }, result.Value);
            }
            return BadRequest(result.Error);

        }
    }
}
