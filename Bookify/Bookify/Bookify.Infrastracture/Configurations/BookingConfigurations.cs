using Bookify.Domain.Booking;
using Bookify.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastracture.Configurations
{
    public sealed class BookingConfigurations : IEntityTypeConfiguration<BookingModel>
    {
        public void Configure(EntityTypeBuilder<BookingModel> builder)
        {
            builder.ToTable("bookings");
            builder.HasKey(b => b.Id);
            builder.OwnsOne(b => b.Duration);
            builder.OwnsOne(b => b.PricePerPeriod, priceBuilder =>
            {
                priceBuilder.Property(price => price.Currency)
                          .HasConversion(currency => currency.Code, value => Currency.FromCode(value));
            });
            builder.OwnsOne(b => b.CleanningFee, cleanningFeeBuilder =>
            {
                cleanningFeeBuilder.Property(fee => fee.Currency)
                          .HasConversion(currency => currency.Code, value => Currency.FromCode(value));
            });
            builder.OwnsOne(b => b.AmenitiesUpCharge, amenitiesUpChargeBuilder =>
            {
                amenitiesUpChargeBuilder.Property(fee => fee.Currency)
                          .HasConversion(currency => currency.Code, value => Currency.FromCode(value));
            });
            builder.OwnsOne(b => b.TotalPrice, totalPriceBuilder =>
            {
                totalPriceBuilder.Property(fee => fee.Currency)
                          .HasConversion(currency => currency.Code, value => Currency.FromCode(value));
            });
            builder.HasOne(b=>b.User)
                .WithMany()
                .HasForeignKey(b=>b.UserId).IsRequired();

            builder.HasOne(b => b.Apartment)
                .WithMany(apartment => apartment.Bookings)
                .HasForeignKey(b => b.ApartmentId).IsRequired();
        }
    }
}
