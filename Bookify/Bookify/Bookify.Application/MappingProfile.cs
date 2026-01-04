using AutoMapper;
using Bookify.Application.Apartment.AddApartment;
using Bookify.Application.Apartment.Dtos;
using Bookify.Application.Booking.Dtos;
using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Bookify.Domain.Shared;

namespace Bookify.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<BookingForCreationDto, BookingModel>()
            //    .ForMember(d => d.Duration, r => r.MapFrom(s => 
            //        DateRange.Create(DateOnly.FromDateTime(s.Duration_Start), DateOnly.FromDateTime(s.Duration_End))
            //    ));

            CreateMap<BookingModel, BookingDto>();
                //.ForMember(d=>d.Duration_Start,r=>r.MapFrom(s=>s.Duration.Start))
                //.ForMember(d => d.Duration_End, r => r.MapFrom(s => s.Duration.End));

            CreateMap<ApartmentModel, ApartmentDto>()
                .ForMember(d=>d.Name,r=>r.MapFrom(s=>s.Name.Value))
                .ForMember(d => d.Description, r => r.MapFrom(s => s.Description.Value));

            CreateMap<ApartmentForCreationDto, AddApartmentCommad>()
                .ForMember(d => d.Price, r => r.MapFrom(s => new Money(s.PriceAmount, Currency.FromCode(s.PriceCurrency))))
                .ForMember(d => d.CleanningFee, r => r.MapFrom(s => new Money(s.CleanningFeeAmount, Currency.FromCode(s.CleanningFeeCurrency))));




        }
    }
}
