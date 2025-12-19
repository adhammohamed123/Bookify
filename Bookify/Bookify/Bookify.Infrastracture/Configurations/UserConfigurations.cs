using Bookify.Domain.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastracture.Configurations
{
    public sealed class UserConfigurations : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u=>u.FirstName)
                .IsRequired().HasMaxLength(100)
                .HasConversion(firsname=> firsname.Value, value => new FirstName(value));

            builder.Property(u => u.LastName)
               .IsRequired().HasMaxLength(100)
               .HasConversion(lastname => lastname.Value, value => new LastName(value));

            builder.Property(u => u.Email)
               .IsRequired().HasMaxLength(100)
               .HasConversion(email => email.Value, value => Domain.User.Email.Create(value));
            
            builder.HasIndex(u => u.Email).IsUnique();
           
            builder.HasMany(u=>u.Reviews)
                .WithOne(r=>r.User)
                .HasForeignKey(r=>r.UserId).IsRequired();
        }
    }
}
