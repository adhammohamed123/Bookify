using AutoMapper;
using Bookify.Application.Apartment.AddApartment;
using Bookify.Application.Apartment.Dtos;
using Bookify.Application.Apartment.GetApartment;
using Bookify.Application.Apartment.SearchApartments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentController(ISender sender,IMapper mapper) : ControllerBase
    {
        [HttpGet("SearchForAvalialbeApartment")]
        public async Task<IActionResult> SearchApartment(DateOnly start,DateOnly end,CancellationToken cancellationToken) 
        {
            var apartmentsQuery = new SearchApartmentsQuery(start,end);
            var result = await sender.Send(apartmentsQuery,cancellationToken);
           
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetApartmentQuery(id);
            var result = await sender.Send(query,cancellationToken);
            return result.IsSuccess? Ok(result.Value): BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewApartment(ApartmentForCreationDto appartmentForCreation,CancellationToken cancellationToken)
        {
            var command = mapper.Map<AddApartmentCommad>(appartmentForCreation);
            var result= await sender.Send(command,cancellationToken);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetAsync), new { id = result.Value.Id  }, result.Value);
           return BadRequest(result.Error) ;
        }
    }
}
