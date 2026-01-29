using Bookify.Application.Booking.CancelBooking;
using Bookify.Application.Booking.CompleteBooking;
using Bookify.Application.Booking.ConfirmBooking;
using Bookify.Application.Booking.Dtos;
using Bookify.Application.Booking.GetBooking;
using Bookify.Application.Booking.RejectBooking;
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
        [HttpPut("Confirm/{id:guid}")]
        public async Task<IActionResult> ConfirmBooking(Guid id,CancellationToken cancellationToken)
        {
            var command = new ConfirmBookingCommand(id);
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess? Ok("Booking Is Confirmed Successfully"): BadRequest(result.Error);  
        }
        [HttpPut("Reject/{id:guid}")]
        public async Task<IActionResult> Reject(Guid id ,CancellationToken cancellationToken)
        {
            var command = new RejectBookingCommand(id);
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? Ok ("Booking Is Rejected Now Successfully") : BadRequest(result.Error);
        }

        [HttpPut("Complete/{id:guid}")]
        public async Task<IActionResult> CompleteBooking(Guid id, CancellationToken cancellationToken)
        {
            var command = new CompleteBookingCommand(id);
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? Ok("Booking Is Completed Successfully") : BadRequest(result.Error);
        }
        [HttpPut("Cancel/{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
        {
            var command = new CancelBookingCommand(id);
            var result = await sender.Send(command, cancellationToken);
            return result.IsSuccess ? Ok("Booking Is Cancelled Now Successfully") : BadRequest(result.Error);
        }
    }
}
