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
}
