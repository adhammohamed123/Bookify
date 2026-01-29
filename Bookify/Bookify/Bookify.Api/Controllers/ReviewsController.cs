using Bookify.Application.Review.CreateReviewCommand;
using Bookify.Application.Review.Dtos;
using Bookify.Application.Review.GetReviewQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController(ISender sender) : ControllerBase
    {

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetReviewById(Guid id,CancellationToken cancellationToken)
        {
            var query = new GetReivewQuery(id);
            var result = await sender.Send(query, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }



        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewForCreationDto dto,CancellationToken cancellationToken)
        {
            var command = new CreateReviewCommand()
            {
                UserId = dto.UserId,
                ApartmentId = dto.ApartmentId,
                BookingId = dto.BookingId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };
            var result = await sender.Send(command, cancellationToken);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetReviewById), new { id = result.Value.Id }, result.Value);
            }
            return BadRequest(result.Error);
        }

       
    }
}
