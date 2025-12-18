using AutoMapper;
using Bookify.Application.Abstractions.Email;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Apartment.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Apartment;
using Bookify.Domain.Apartment.Events;
using MediatR;

namespace Bookify.Application.Apartment.AddApartment
{
    internal sealed class AddApartmentCommandHandler(IRepositoryManager repositoryManager,IMapper mapper)
        : 
        ICommandHandler<AddApartmentCommad, ApartmentDto>
    {
        private readonly IRepositoryManager repositoryManager = repositoryManager;
        private readonly IMapper mapper = mapper;

        public async Task<Result<ApartmentDto>> Handle(AddApartmentCommad request, CancellationToken cancellationToken)
        {
            var apartment =ApartmentModel.Create(
                new Name(request.Name),
                new Description(request.Description),
                new Address(request.State,request.City,request.Street,request.ZipCode,request.Country),
                request.Price,
                request.CleanningFee,
                request.Amenities
                );

            if (apartment.IsFaliure)
            {
                return Result.Failure<ApartmentDto>(apartment.Error);
            }
            await repositoryManager.ApartmentRepository.AddNewApartmentAsync(apartment.Value, cancellationToken);
            await  repositoryManager.SaveChangesAsync(cancellationToken);
            var apartmentDto = mapper.Map<ApartmentDto>(apartment.Value);
            return Result.Success(apartmentDto);
        }
    }
    internal sealed class ApartmentCreatedDomainEventHandler : INotificationHandler<ApartmentCreatedDomainEvent>
    {
        public Task Handle(ApartmentCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // send notification or log
            return Task.CompletedTask;
        }
    }
}
