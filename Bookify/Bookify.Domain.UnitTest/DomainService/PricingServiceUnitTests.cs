using Bookify.Domain.Booking;
using Bookify.Domain.DomainServices;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTest.Apartment;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.UnitTest.DomainService
{
    public class PricingServiceUnitTests
    {
        [Fact]
        public void Calc_Should_Return_Valid_Pricing()
        {
            // arrage
            var apartment = ApartmentData.Create(); // ( day = 100$ ) + (10$ cleaningfee) + ( amainities percentage = 8% ~ 32$)
            var duration = DateRange.Create(new (2026,1,6), new(2026,1,10));
            var expectedPricing = new Money(442m , apartment.Price.Currency);
            // act
            var pricingDetials = PricingService.Calc(apartment, duration);
            // assert
            duration.Value.LengthInDays.Should().Be(4);
            pricingDetials.TotalPrice.Should().Be(expectedPricing);
            pricingDetials.CleanningFee.Should().Be(apartment.CleanningFee);
            pricingDetials.PricePerPeriod.Should().Be(new Money(400m,apartment.Price.Currency));
        }
    }
}
