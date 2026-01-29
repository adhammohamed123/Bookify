using Bookify.Domain.Apartment;

namespace Bookify.Application.Apartment.Dtos
{
    public record ApartmentForCreationDto
    {
        public required string Name { get; set; }
        public required string Description {  get; set; }
        public required string State { get; set; }
        public required string City { get; set; }
        public required string Street {  get; set; }
        public  required string ZipCode { get; set; }
        public required string Country { get; set; }
        public required decimal PriceAmount { get; set; }
        public required string PriceCurrency { get; set; }
        public required decimal CleanningFeeAmount { get; set; }
        public required string CleanningFeeCurrency { get; set; }
        public required ICollection<Amenity> Amenities {  get; set; }
    }
}
