using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking.Events;
using MediatR;

namespace Bookify.Application.Booking.ConfirmBooking
{
    internal sealed class BookingWasConfirmedDomainEventHandler(IEmailSender emailSender,IRepositoryManager repositoryManager) : INotificationHandler<BookingWasConfirmedDomainEvent>
    {
        public async Task Handle(BookingWasConfirmedDomainEvent notification, CancellationToken cancellationToken)
        {
           var booking = await repositoryManager.BookingRepository.GetBookingAsync(notification.BookingId,trackChanges: false);
            if(booking is null) // in actual world never be null here
                return;
           
            var user = await repositoryManager.UserRepository.GetUserAsync(booking.UserId,trackChanges:false);
            if(user is null)// in actual world never be null here
                return;

           await emailSender.SendEmailAsync(
               user.Email, 
               "New Booking Is Confirmed!", 
               $"Dear Customer:{user.FirstName.Value}\n " +
               $"Thank you for Confirming This New Booking \n" +
               $"{booking.Duration.Start}::{booking.Duration.End} = {booking.TotalPrice}{booking.TotalPrice.Currency.Code}",
               isHtml:false,cancellationToken);
        }
    }

}
