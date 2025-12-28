using Bogus;
using Bookify.Domain.Apartment;
using Bookify.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastracture.Configurations
{
    public sealed class ApartmentConfigurations : IEntityTypeConfiguration<ApartmentModel>
    {
        public void Configure(EntityTypeBuilder<ApartmentModel> builder)
        {
            builder.ToTable("apartments");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                .IsRequired().HasMaxLength(200)
                .HasConversion(name => name.Value, value => new Name(value));
            builder.Property(a => a.Description)
                .IsRequired().HasMaxLength(1000)
                .HasConversion(description => description.Value, value => new Description(value));

            builder.OwnsOne(a => a.Address);
            builder.OwnsOne(a => a.Price, priceBuilder =>
            {
                priceBuilder.Property(price=>price.Currency)
                          .HasConversion(currency => currency.Code,value=> Currency.FromCode(value));
            });
            builder.OwnsOne(a => a.CleanningFee, cleanningFeeBuilder =>
            {
                cleanningFeeBuilder.Property(fee => fee.Currency)
                          .HasConversion(currency => currency.Code, value => Currency.FromCode(value));
            });

            builder.Property<uint>("Version").IsRowVersion();

            builder.HasMany(apartment=>apartment.Bookings)
                .WithOne(booking=>booking.Apartment)
                .HasForeignKey(booking=>booking.ApartmentId).IsRequired();
          
            builder.HasMany(apartment => apartment.Reviews)
                .WithOne(review => review.Apartment)
                .HasForeignKey(review => review.ApartmentId)
                .IsRequired();


        }
    }

    public static class DamyData
    {
        public static IEnumerable<ApartmentModel> GetApartmentDemyData()
        {
            var faker = new Faker();
            var apartments = new List<ApartmentModel>();

            for (int i = 0; i < 100; i++)
            {
                var apartment = ApartmentModel.Create(
                    new Name(faker.Company.CompanyName()),
                    new Description(faker.Lorem.Paragraph()),
                    new Address(
                        faker.Address.StreetAddress(),
                        faker.Address.City(),
                        faker.Address.State(),
                        faker.Address.ZipCode(),
                        faker.Address.Country()
                    ),
                    new Money(faker.Random.Decimal(50, 500), Currency.FromCode("USD")),
                    new Money(faker.Random.Decimal(10, 100), Currency.FromCode("USD")),
                    [
                        Amenity.Wifi,
                        Amenity.AirConditioning,
                        Amenity.Parking
                    ]
                ).Value;
                apartments.Add(apartment);
            }
            return apartments;

        }
       


    }
}
