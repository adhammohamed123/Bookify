using AutoMapper;
using Bookify.Application.Booking.Dtos;
using Bookify.Domain.Booking;

namespace Bookify.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookingForCreationDto, BookingModel>()
                .ForMember(d => d.Duration, r => r.MapFrom(s => 
                    DateRange.Create(DateOnly.FromDateTime(s.Duration_Start), DateOnly.FromDateTime(s.Duration_End))
                ));

            CreateMap<BookingModel, BookingDto>()
                .ForMember(d=>d.Duration_Start,r=>r.MapFrom(s=>s.Duration.Start))
                .ForMember(d => d.Duration_End, r => r.MapFrom(s => s.Duration.End));
        }
    }
}
