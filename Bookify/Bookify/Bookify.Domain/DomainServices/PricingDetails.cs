using Bookify.Domain.Shared;

namespace Bookify.Domain.DomainServices
{
   
        public record PricingDetails(Money PricePerPeriod,Money CleanningFee,Money AmenitiesUpCharge,Money TotalPrice);
    
}
