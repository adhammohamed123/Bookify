using Bookify.Application.Apartment.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentController(ISender sender) : ControllerBase
    {
        private readonly ISender sender = sender;

        [HttpGet("SearchForAvalialbeApartment")]
        public async Task<IActionResult> SearchApartment(DateOnly start,DateOnly end,CancellationToken cancellationToken) 
        {
            var apartmentsQuery = new SearchApartmentsQuery(start,end);
            var result = await sender.Send(apartmentsQuery,cancellationToken);
           
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }
    }
}
