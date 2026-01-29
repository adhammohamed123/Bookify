using AutoMapper;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartment.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking;

namespace Bookify.Application.Apartment.SearchApartments
{
    public record SearchApartmentsQuery(DateOnly Start, DateOnly End) : ICachedQuery<IReadOnlyList<ApartmentDto>>
    {
        public string CacheKey => $"Apartments:Search:Start{Start}_End{End}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
    }

    internal sealed class SearchApartmentsHandler(IRepositoryManager repositoryManager,IMapper mapper,IDateTimeProvider dateTimeProvider)
        : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentDto>>
    {
        private readonly IRepositoryManager repositoryManager = repositoryManager;
        private readonly IMapper mapper = mapper;
        private readonly IDateTimeProvider dateTimeProvider = dateTimeProvider;

        public async Task<Result<IReadOnlyList<ApartmentDto>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
        {
            /// validate start and end date we will use fluent validation in future ,, ==> here we can not validate this because we only validate commands not queries
            if (request.Start >= request.End)
            {
                return Result.Failure<IReadOnlyList<ApartmentDto>>(DateRange.InValid);
            }
            if( request.Start < DateOnly.FromDateTime(dateTimeProvider.UtcNow))
            {
                return Result.Failure<IReadOnlyList<ApartmentDto>>(new("SearchParameters.InValid", "Start cannot be in the past"));
            }
            var duration = DateRange.Create(request.Start, request.End);
            var apartments =  repositoryManager.ApartmentRepository.GetAllApartmentsAvailableInAsync(duration,false).ToList();

           var apartmentDtos= mapper.Map<IReadOnlyList<ApartmentDto>>(apartments);
           return Result.Success(apartmentDtos);
        }
    }
}
