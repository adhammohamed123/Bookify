using AutoMapper;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Booking.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Bookify.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking.ReserveBooking
{
    public record ReserveBookingCommand(BookingForCreationDto BookingForCreationDto) : ICommand<Guid>;


    internal sealed class ReserveBookingCommandHandler(IRepositoryManager repositoryManager,IDateTimeProvider dateTimeProvider,IMapper mapper) 
        : BookingBaseHandler(repositoryManager,mapper), 
        ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IDateTimeProvider dateTimeProvider = dateTimeProvider;

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await CheckUserExistanceAsync(request.BookingForCreationDto.UserId);

            if(user.IsFaliure)
            {
                return Result.Failure<Guid>(user.Error);
            }

            var apartment = await CheckApartmentExistanceAsync(request.BookingForCreationDto.ApartmentId);

            if(apartment.IsFaliure)
            {
                return Result.Failure<Guid>(apartment.Error);
            }

            var isOverlapping = await repositoryManager.BookingRepository.IsOverLappedBooking(request.BookingForCreationDto.ApartmentId, request.BookingForCreationDto.Duration_Start , request.BookingForCreationDto.Duration_End,cancellationToken);
            if(isOverlapping)
            {
                return Result.Failure<Guid>(BookingErrors.BookingDateOverLapped);
            }
            DateRange dateRange = DateRange.Create(DateOnly.FromDateTime (request.BookingForCreationDto.Duration_Start), DateOnly.FromDateTime (request.BookingForCreationDto.Duration_End));

            var bookingResult = BookingModel.Reserve(user.Value.Id, apartment,dateRange, dateTimeProvider.UtcNow);
               
            if (bookingResult.IsFaliure)
            {
                return Result.Failure<Guid>(BookingErrors.FailedProcess);
            }
           
           await repositoryManager.BookingRepository.AddBookingAsync(bookingResult,cancellationToken);
           await repositoryManager.SaveChangesAsync(cancellationToken);
           return Result.Success(bookingResult.Value.Id);
               
        }
    }

}
