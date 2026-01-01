using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Email;
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

namespace Bookify.Application.Booking.CompleteBooking
{

    public record CompleteBookingCommand(Guid BookingId) : ICommand;

    internal sealed class CompleteBookingCommandHandler(IRepositoryManager repositoryManager,IDateTimeProvider dateTimeProvider) 
        : BookingBaseHandler(repositoryManager), ICommandHandler<CompleteBookingCommand>
    {
        public async Task<Result> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
        {
            var BookingExistance = await CheckBookingExistance(request.BookingId, track: true);
            
            if (BookingExistance.IsFaliure)
                return BookingExistance;
            var CompleteBookingResult = BookingExistance.Value.Complete(dateTimeProvider.UtcNow);
           
            if(CompleteBookingResult.IsFaliure)
                return CompleteBookingResult;

            await repositoryManager.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }


    internal sealed class BookingWasCompletedDomainEventHandler(IRepositoryManager repositoryManager,IEmailSender emailSender) : INotificationHandler<BookingWasCompletedDomainEvent>
    {
        public async Task Handle(BookingWasCompletedDomainEvent notification, CancellationToken cancellationToken)
        {
            var booking = await repositoryManager.BookingRepository.GetBookingAsync(notification.Id,false);
            if(booking == null) return;
            var user = await repositoryManager.UserRepository.GetUserAsync(booking.UserId, false);
            if(user == null) return;
            await emailSender.SendEmailAsync(user.Email,
                "New Booking IS Completed",
                $"Congrats,{user.FirstName.Value}, you are complete a new Booking that <b> {booking.Duration.Start}-->{booking.Duration.End} = {booking.TotalPrice.Amount}{booking.TotalPrice.Currency.Code} </b>", true, cancellationToken);
        }
    }
}
