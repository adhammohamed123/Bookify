using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartment;
using Bookify.Domain.Booking.Events;
using Bookify.Domain.DomainServices;
using Bookify.Domain.Shared;
using Bookify.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.Booking
{
    public sealed class BookingModel : Entity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private BookingModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {

        }
        private BookingModel(
            Guid id,
            Guid userId, 
            Guid apartmentId, 
            DateRange duration, 
            Money pricePerPeriod, 
            Money cleanningFee, 
            Money amenitiesUpCharge, 
            Money totalPrice, 
            DateTime createdAtUtc):base(id)
        {
            UserId = userId;
            ApartmentId = apartmentId;
            Duration = duration;
            PricePerPeriod = pricePerPeriod;
            CleanningFee = cleanningFee;
            AmenitiesUpCharge = amenitiesUpCharge;
            TotalPrice = totalPrice;
            CreatedAtUtc = createdAtUtc;
        }

       
        public Guid UserId { get; private set; }
        public Guid ApartmentId { get; private set; }
        public DateRange Duration { get; private set; }


        public Money PricePerPeriod { get; private set; }
        public Money CleanningFee { get; private set; }
        public Money AmenitiesUpCharge { get; private set; }
        public Money TotalPrice { get;private set; }

        public BookingStatus Status { get; private set; }

        public DateTime CreatedAtUtc { get;private set; }
        public DateTime? ConfirmedAtUtc { get;private set; }
        public DateTime? RejectedAtUtc { get;private set; }
        public DateTime? CompletedAtUtc { get;private set; }
        public DateTime? CanceledAtUtc { get;private set; }

        public UserModel? User { get; set; }
        public ApartmentModel? Apartment { get; set; }
        public static Result<BookingModel> Reserve(Guid userId, ApartmentModel apartment, DateRange duration, DateTime createdAtUtc)
        {
            var pricingDetails = PricingService.Calc(apartment, duration);
            var booking = new BookingModel(Guid.NewGuid(), userId, apartment.Id, duration, pricingDetails.PricePerPeriod, pricingDetails.CleanningFee, pricingDetails.AmenitiesUpCharge, pricingDetails.TotalPrice, createdAtUtc)
            {
                Status = BookingStatus.Reserved
            };
            apartment.LastBookedOnUtc = createdAtUtc;
            
            booking.RaiseDomainEvent(new BookingWasCreatedDomainEvent(booking.Id));
           
           return booking;
        }

        public Result Confirm(DateTime utcNow)
        {
            if (Status != BookingStatus.Reserved)
                return Result.Failure(BookingErrors.NotReserved);

            Status= BookingStatus.Confirmed;
            ConfirmedAtUtc = utcNow;

            RaiseDomainEvent(new BookingWasConfirmedDomainEvent(Id));

            return Result.Success();
        }
        public Result Reject(DateTime utcNow)
        {
            if (Status != BookingStatus.Reserved)
                return Result.Failure(BookingErrors.NotReserved);

            Status = BookingStatus.Reserved;
            RejectedAtUtc = utcNow;

            RaiseDomainEvent(new BookingWasRejectedDomainEvent(Id));

            return Result.Success();
        }
        public Result Complete(DateTime utcNow)
        {
            if (Status != BookingStatus.Confirmed)
                return Result.Failure(BookingErrors.NotConfirmed);
            Status = BookingStatus.Completed;
            CompletedAtUtc = utcNow;
            RaiseDomainEvent(new BookingWasCompletedDomainEvent(Id));
            return Result.Success();
        }
        public Result Cancel(DateTime utcNow)
        {
            if (Status != BookingStatus.Confirmed)
                return Result.Failure(BookingErrors.NotConfirmed);
            Status = BookingStatus.Canceled;
            CanceledAtUtc = utcNow;
            RaiseDomainEvent(new BookingWasCanceledDomainEvent(Id));
            return Result.Success();
        }

    }

}
