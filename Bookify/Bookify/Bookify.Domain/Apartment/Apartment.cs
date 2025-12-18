using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartment.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Apartment
{
    // anemic domain model
    public sealed class ApartmentModel:Entity
    {
      
        private ApartmentModel(
            Guid id,
            Name name, 
            Description description, 
            Address address, 
            Money price, 
            Money cleanningFee,
            DateTime? lastBookedOnUtc,
            ICollection<Amenity> amenities): base(id)
        {
            Name = name;
            Description = description;
            Address = address;
            Price = price;
            CleanningFee = cleanningFee;
           LastBookedOnUtc = lastBookedOnUtc;
            Amenities = amenities;
        }

        public Name Name { get; private set; }
        public Description Description { get; private set; }
        public Address Address { get; private set; }
        public Money Price { get; private set; }
        public Money CleanningFee { get; private set; }
        public DateTime? LastBookedOnUtc { get;internal set; }
        public ICollection<Amenity> Amenities { get; private set; } = [];

        public static Result<ApartmentModel> Create(
            Name name,
            Description description,
            Address address,
            Money price,
            Money cleanningFee,
            DateTime? lastBookedOnUtc,
            ICollection<Amenity> amenities)
        {
            // add any business rules or validations here
            if (amenities == null || amenities.Count==0)
            {
                return Result.Failure<ApartmentModel>(ApartmentErrors.NoAmenities);
            }
            var apartment = new ApartmentModel(
                Guid.NewGuid(),
                name,
                description,
                address,
                price,
                cleanningFee,
                lastBookedOnUtc,
                amenities);

            apartment.RaiseDomainEvent(new ApartmentCreatedDomainEvent(apartment.Id));

            return Result.Success(apartment);
        }

    }

}
