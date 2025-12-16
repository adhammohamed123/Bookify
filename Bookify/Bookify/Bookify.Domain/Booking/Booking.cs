using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartment;
using Bookify.Domain.Booking.Events;
using Bookify.Domain.DomainServices;
using Bookify.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.Booking
{
    public sealed class Booking : Entity
    {
        private Booking(
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


        public static Result<Booking> Reserve(Guid userId, ApartmentModel apartment, DateRange duration, DateTime createdAtUtc)
        {
            var pricingDetails = PricingService.Calc(apartment, duration);
            var booking= new Booking(Guid.NewGuid(), userId,apartment.Id,duration,pricingDetails.PricePerPeriod,pricingDetails.CleanningFee,pricingDetails.AmenitiesUpCharge,pricingDetails.TotalPrice,createdAtUtc);
            booking.Status = BookingStatus.Reserved;
            apartment.LastBookedOnUtc = createdAtUtc;
            booking.RaiseDomainEvent(new BookingWasCreatedDomainEvent(booking.Id));
           
           return booking;
        }

        public Result<Booking> Confirm(DateTime utcNow)
        {
            if (Status != BookingStatus.Reserved)
                return Result<Booking>.Failure(BookingErrors.NotReserved);

            Status= BookingStatus.Confirmed;
            ConfirmedAtUtc = utcNow;

            RaiseDomainEvent(new BookingWasConfirmedDomainEvent(Id));

            return Result<Booking>.Success(this);
        }
        public Result<Booking> Reject(DateTime utcNow)
        {
            if (Status != BookingStatus.Reserved)
                return Result<Booking>.Failure(BookingErrors.NotReserved);

            Status = BookingStatus.Reserved;
            RejectedAtUtc = utcNow;

            RaiseDomainEvent(new BookingWasRejectedDomainEvent(Id));

            return Result<Booking>.Success(this);
        }


    }

    public static class BookingErrors
    {
        public static Error NotReserved => new("Booking.NotReserved", "this booking is no longer be pending");   
            
    }

    public record DateRange 
    {
        private DateRange(DateOnly start, DateOnly end)
        {
            Start = start;
            End = end;
        }

        public DateOnly Start { get; private set; }
        public DateOnly End { get; private set; }
        public int LengthInDays => End.DayNumber - Start.DayNumber;

        private static Error InValid => new("DateRange.InValid", "invalid date range start can not exceed end date");

        public static Result<DateRange> Create(DateOnly start, DateOnly end)
        {
            if(start>end)
            {
                return Result<DateRange>.Failure(InValid) ;
            }
            return new DateRange(start, end);
        }
    }

}
