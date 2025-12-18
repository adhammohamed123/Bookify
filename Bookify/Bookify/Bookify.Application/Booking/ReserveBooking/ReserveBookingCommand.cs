using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Bookify.Domain.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking.ReserveBooking
{
    public record ReserveBookingCommand(
        Guid UserId,
        Guid ApartmentId,
        DateTime StartDateUtc,
        DateTime EndDateUtc) : ICommand<Guid>;

    internal sealed class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.ApartmentId).NotEmpty().WithMessage("ApartmentId is required.");
            RuleFor(x => x.StartDateUtc).LessThan(x => x.EndDateUtc).WithMessage("Start Date must be earlier than End Date.");
        }
    }

    internal sealed class ReserveBookingCommandHandler(IRepositoryManager repositoryManager,IDateTimeProvider dateTimeProvider) 
        : BookingBaseHandler(repositoryManager), 
        ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IDateTimeProvider dateTimeProvider = dateTimeProvider;

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await CheckUserExistanceAsync(request.UserId);

            if(user.IsFaliure)
            {
                return Result.Failure<Guid>(user.Error);
            }

            var apartment = await CheckApartmentExistanceAsync(request.ApartmentId);

            if(apartment.IsFaliure)
            {
                return Result.Failure<Guid>(apartment.Error);
            }
            #region Here we need optimistic concurrency
            var isOverlapping = await repositoryManager.BookingRepository.IsOverLappedBooking(request.ApartmentId, request.StartDateUtc, request.EndDateUtc,cancellationToken);
            if(isOverlapping)
            {
                return Result.Failure<Guid>(BookingErrors.BookingDateOverLapped);
            }
            DateRange dateRange = DateRange.Create(DateOnly.FromDateTime (request.StartDateUtc), DateOnly.FromDateTime (request.EndDateUtc)); // will not throw error because already validated in validator

            var bookingResult = BookingModel.Reserve(user.Value.Id, apartment,dateRange, dateTimeProvider.UtcNow); // raise domain event so we need domain event handler

            if (bookingResult.IsFaliure)
            {
                return Result.Failure<Guid>(BookingErrors.FailedProcess);
            }
           
           await repositoryManager.BookingRepository.AddBookingAsync(bookingResult,cancellationToken);
           await repositoryManager.SaveChangesAsync(cancellationToken);
            #endregion
            return Result.Success(bookingResult.Value.Id);
               
        }
    }

}
