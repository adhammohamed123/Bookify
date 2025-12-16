using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Bookify.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.DomainServices
{
    public  class PricingService
    {
        public static PricingDetails Calc(ApartmentModel apartment,DateRange duration)
        {
            var currency= apartment.Price.Currency;
            var PricePerPeriod= new Money(apartment.Price.Amount * duration.LengthInDays, currency);

            decimal AminityPercentage = 0;
            foreach (var aminety in apartment.Amenities)
            {
                AminityPercentage += aminety switch
                {
                    Amenity.MountainView or Amenity.GardenView => 0.02m,
                    Amenity.Wifi or Amenity.Washer or Amenity.Dryer => 0.03m,
                    Amenity.Parking=>0.04m,
                    Amenity.AirConditioning=>0.05m,
                    _ => 0m
                };
            }
            var amenitiesUpCharge = Money.Zero(currency);
            if (AminityPercentage > 0)
            {
                amenitiesUpCharge += new Money(PricePerPeriod.Amount * AminityPercentage, currency);
            }

            var totalprice = PricePerPeriod + apartment.CleanningFee + amenitiesUpCharge;
            return new PricingDetails(PricePerPeriod,apartment.CleanningFee,amenitiesUpCharge,totalprice)   ;
        }
    }
}
