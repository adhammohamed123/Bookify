using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking.CancelBooking
{

    public record CancelBookingCommand(Guid BookingId):ICommand;


    internal sealed class CancelBookingCommandHandler(IRepositoryManager repositoryManager,IDateTimeProvider dateTimeProvider) :
        BookingBaseHandler(repositoryManager),
        ICommandHandler<CancelBookingCommand>
    {
        public async Task<Result> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var BookingExistance = await CheckBookingExistance(request.BookingId, true);
            if (BookingExistance.IsFaliure)
                return BookingExistance;

            var cancellationResult =  BookingExistance.Value.Cancel(dateTimeProvider.UtcNow); 
            if (cancellationResult.IsFaliure)
                return cancellationResult;

           await  repositoryManager.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }


    internal sealed class BookingWasCanceledDomainEventHandler : INotificationHandler<BookingWasCanceledDomainEvent>
    {
        public async Task Handle(BookingWasCanceledDomainEvent notification, CancellationToken cancellationToken)
        {
           await Task.CompletedTask;
        }
    }


}
