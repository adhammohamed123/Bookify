using AutoMapper;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartment.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking;

namespace Bookify.Application.Apartment.SearchApartments
{
     public record SearchApartmentsQuery(DateTime Start,DateTime End):IQuery<IReadOnlyList<ApartmentDto>>;
   
    internal sealed class SearchApartmentsHandler(IRepositoryManager repositoryManager,IMapper mapper)
        : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentDto>>
    {
        private readonly IRepositoryManager repositoryManager = repositoryManager;
        private readonly IMapper mapper = mapper;

        public async Task<Result<IReadOnlyList<ApartmentDto>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
        {
            /// validate start and end date we will use fluent validation in future ,, ==> here we can not validate this because we only validate commands not queries
            if (request.Start >= request.End)
            {
                return Result.Failure<IReadOnlyList<ApartmentDto>>(DateRange.InValid);
            }
            var duration = DateRange.Create(DateOnly.FromDateTime(request.Start), DateOnly.FromDateTime(request.End));
            var apartments =  repositoryManager.ApartmentRepository.GetAllApartmentsAvailableInAsync(duration,false).ToList();

           var apartmentDtos= mapper.Map<IReadOnlyList<ApartmentDto>>(apartments);
           return Result.Success(apartmentDtos);
        }
    }
}
