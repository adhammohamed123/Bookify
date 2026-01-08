using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartment;
using Bookify.Domain.Apartment.Events;
using FluentAssertions;

namespace Bookify.Domain.UnitTest.Apartment
{
    public class Apartment_UnitTests
    {
        [Fact]
        public void Create_Should_Return_Fauiler_When_Amenities_Is_Null()
        {
            // Arrage
            // Act
            var apartmentResult = ApartmentModel.Create(ApartmentData.Name, ApartmentData.Description, ApartmentData.Address, ApartmentData.price, ApartmentData.CleanningFee, null);
            // Assert
            apartmentResult.IsFaliure.Should().BeTrue();
            apartmentResult.Error.Should().Be(ApartmentErrors.NoAmenities);
        }
        [Fact]
        public void Create_Should_Set_PropertiesValues_With_Success_Result_And_No_Error()
        {
            // Arrage
            // Act
            var apartmentResult = ApartmentModel.Create(ApartmentData.Name, ApartmentData.Description, ApartmentData.Address, ApartmentData.price, ApartmentData.CleanningFee, ApartmentData.amenities);
            // Assert
           apartmentResult.IsSuccess.Should().BeTrue();
           apartmentResult.Error.Should().Be(Error.NoError);
           apartmentResult.Value.Name.Should().Be(ApartmentData.Name);
           apartmentResult.Value.Description.Should().Be(ApartmentData.Description);
           apartmentResult.Value.Address.Should().Be(ApartmentData.Address);
           apartmentResult.Value.Price.Should().Be(ApartmentData.price);
           apartmentResult.Value.CleanningFee.Should().Be(ApartmentData.CleanningFee);
           apartmentResult.Value.Amenities.Should().BeEquivalentTo(ApartmentData.amenities);
        }
        [Fact]
        public void Create_Should_Rasise_ApartmentCreatedDomainEvent()
        {
            // Arrage
            // Act
            var apartmentResult = ApartmentModel.Create(ApartmentData.Name, ApartmentData.Description, ApartmentData.Address, ApartmentData.price, ApartmentData.CleanningFee, ApartmentData.amenities);
            var domainEvent= apartmentResult.Value.GetDomainEvents.OfType<ApartmentCreatedDomainEvent>().SingleOrDefault();
            // Assert
            domainEvent.Should().NotBeNull();
        }
    }
}
