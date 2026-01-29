using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Email;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking.RejectBooking
{

    public record RejectBookingCommand(Guid BookingId) : ICommand;

    internal sealed class RejectBookingCommandHandler(IRepositoryManager repositoryManager,IDateTimeProvider dateTimeProvider) :
        BookingBaseHandler(repositoryManager),
        ICommandHandler<RejectBookingCommand>
    {
        public async Task<Result> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
        {
            var BookingExistance = await base.CheckBookingExistance(request.BookingId , track: true);
            
            if (BookingExistance.IsFaliure)
                return Result.Failure(BookingExistance.Error);

            var RejectingBookingResult =  BookingExistance.Value.Reject(dateTimeProvider.UtcNow);
            
            if (RejectingBookingResult.IsFaliure)
                return Result.Failure(RejectingBookingResult.Error);

           await repositoryManager.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }


    internal sealed class BookingWasRejectedDomainEventHandler(IRepositoryManager repositoryManager,IEmailSender emailSender) : INotificationHandler<BookingWasRejectedDomainEvent>
    {
        public async Task Handle(BookingWasRejectedDomainEvent notification, CancellationToken cancellationToken)
        {
            var booking = await repositoryManager.BookingRepository.GetBookingAsync(notification.BookingId, false);
            if(booking is null)
                return;
            var user = await repositoryManager.UserRepository.GetUserAsync(booking.UserId, false);
            if (user is null) return;

            var apartment = await repositoryManager.ApartmentRepository.GetApartmentAsync(booking.ApartmentId, false);
            if(apartment is null) return;
            await emailSender.SendEmailAsync(user.Email,
                "Booking Was Rejected",
                $"we are sorry, but your booking To {apartment.Name} \nIn: {apartment.Address}\n" +
                $"From Period {booking.Duration.Start} : To {booking.Duration.End}" +
                $"Was Rejected From Our System,\nBest Regards",false,cancellationToken);


        }
    }
}
