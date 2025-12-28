using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking.Events;
using MediatR;

namespace Bookify.Application.Booking.ReserveBooking
{
    internal sealed class BookingWasCreatedDomainEventHandler(IEmailSender emailSender,IRepositoryManager repositoryManager) : INotificationHandler<BookingWasCreatedDomainEvent>
    {
        private readonly IEmailSender emailSender = emailSender;
        private readonly IRepositoryManager repositoryManager = repositoryManager;

        public async Task Handle(BookingWasCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            // send notification email to user or admin
            var booking=await repositoryManager.BookingRepository.GetBookingAsync(domainEvent.BookingId, false);
           
            if (booking == null) 
                return;

            var user= await repositoryManager.UserRepository.GetUserAsync(booking.UserId, false);
            if (user == null)
                return;

            await  emailSender.SendEmailAsync(user.Email,"Your are Reserving a New Booking to ","you have 10 minutes to Confirm this Booking",false,cancellationToken);
        }
    }

}
