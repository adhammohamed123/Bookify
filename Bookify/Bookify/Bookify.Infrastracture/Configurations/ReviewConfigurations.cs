using Bookify.Domain.Review;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastracture.Configurations
{
    public sealed class ReviewConfigurations : IEntityTypeConfiguration<ReviewModel>
    {
        public void Configure(EntityTypeBuilder<ReviewModel> builder)
        {
            builder.ToTable("reviews");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Rating)
                .IsRequired()
                .HasConversion(rating=> rating.Value,value=> Rating.Create(value));
            builder.Property(r => r.Comment)
                .HasMaxLength(1000)
                .HasConversion(comment=>comment.value,value=> new Comment(value));
          
            builder.HasOne(r => r.User)
                .WithMany(u=>u.Reviews)
                .HasForeignKey(r => r.UserId)
                .IsRequired();
            
            builder.HasOne(r => r.Apartment)
                .WithMany(a=>a.Reviews)
                .HasForeignKey(r => r.ApartmentId)
                .IsRequired();
            builder.HasOne(r => r.Booking)
                .WithMany()
                .HasForeignKey(r => r.BookingId)
                .IsRequired();
        }
    }
}
