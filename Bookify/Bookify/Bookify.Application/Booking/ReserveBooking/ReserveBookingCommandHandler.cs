using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking;

namespace Bookify.Application.Booking.ReserveBooking
{
    internal sealed class ReserveBookingCommandHandler(IRepositoryManager repositoryManager,IDateTimeProvider dateTimeProvider) 
        : BookingBaseHandler(repositoryManager), 
        ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IDateTimeProvider dateTimeProvider = dateTimeProvider;

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await CheckUserExistanceAsync(request.UserId,false);

            if(user.IsFaliure)
            {
                return Result.Failure<Guid>(user.Error);
            }

            var apartment = await CheckApartmentExistanceAsync(request.ApartmentId,false);

            if(apartment.IsFaliure)
            {
                return Result.Failure<Guid>(apartment.Error);
            }
            #region Here we need optimistic concurrency
             DateRange dateRange = DateRange.Create(DateOnly.FromDateTime (request.StartDateUtc), DateOnly.FromDateTime (request.EndDateUtc)); // will not throw error because already validated in validator
           
            var isOverlapping = await repositoryManager.BookingRepository.IsOverLappedBooking(request.ApartmentId,duration:dateRange ,cancellationToken);
            if(isOverlapping)
            {
                return Result.Failure<Guid>(BookingErrors.BookingDateOverLapped);
            }

            try
            {
                var bookingResult = BookingModel.Reserve(user.Value.Id, apartment, dateRange, dateTimeProvider.UtcNow); // raise domain event so we need domain event handler

                await repositoryManager.BookingRepository.AddBookingAsync(bookingResult);
                await repositoryManager.SaveChangesAsync(cancellationToken);
                #endregion
                return Result.Success(bookingResult.Value.Id);
            }
            catch (ConcurrencyException)
            {
                return Result.Failure<Guid>(BookingErrors.BookingDateOverLapped);
            }
               
        }
    }

}
