using AutoMapper;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartment.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Apartment;

namespace Bookify.Application.Apartment.GetApartment
{

    public record GetApartmentQuery(Guid ApartmentId) : ICachedQuery<ApartmentDto>
    {
        public string CacheKey => $"Apartments:{ApartmentId}";

        public TimeSpan? Expiration => null;
    }


    internal sealed class GetApartmentQueryHandler(IRepositoryManager repositoryManager,IMapper mapper) : IQueryHandler<GetApartmentQuery, ApartmentDto>
    {
        public async Task<Result<ApartmentDto>> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
        {
            var apartmentExistance= await repositoryManager.ApartmentRepository.GetApartmentAsync(request.ApartmentId,trackChanges: false);

            if (apartmentExistance is null)
                return Result.Failure<ApartmentDto>(ApartmentErrors.ApartmentNotFound);

            var mapped= mapper.Map<ApartmentDto>(apartmentExistance);

            return Result.Success(mapped);
        }
    }
}
