using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking;

namespace Bookify.Application.Booking.ConfirmBooking
{
    internal sealed class ConfirmBookingCommandHandler(IRepositoryManager repositoryManager,IDateTimeProvider dateTimeProvider)
        : BookingBaseHandler(repositoryManager),
        ICommandHandler<ConfirmBookingCommand>
    {
        public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            var bookingExistance= await  base.CheckBookingExistance(request.BookingId, track: true);
           // var booking=await repositoryManager.BookingRepository.GetBookingAsync(request.BookingId,trackChanges: true); 
           
            if(bookingExistance.IsFaliure)
                return Result.Failure(bookingExistance.Error);

           var resultOfConfirmation=  bookingExistance.Value.Confirm(dateTimeProvider.UtcNow);

            if (resultOfConfirmation.IsFaliure)
                return Result.Failure(resultOfConfirmation.Error);

            await  repositoryManager.SaveChangesAsync(cancellationToken); // don't forget DomainEventHandler

            return Result.Success();
        }
    }

}
