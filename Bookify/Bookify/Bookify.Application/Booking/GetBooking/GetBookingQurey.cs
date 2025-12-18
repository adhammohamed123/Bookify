using AutoMapper;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Booking.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking.GetBooking
{
    public record GetBookingQurey(Guid Id) : IQuery<BookingDto>;

    internal sealed class GetBookingQureyHandler(IRepositoryManager repositoryManager,IMapper mapper)
        : BookingBaseHandler(repositoryManager,mapper),
        IQueryHandler<GetBookingQurey, BookingDto>
    {
        public async Task<Result<BookingDto>> Handle(GetBookingQurey request, CancellationToken cancellationToken)
        {
            var bookingResult = await CheckBookingExistance(request.Id);
            if (bookingResult.IsFaliure)
            {
                return Result.Failure<BookingDto>(bookingResult.Error);
            }
           
            var bookingDto = mapper.Map<BookingDto>(bookingResult.Value);

            return Result.Success(bookingDto);
        }
    }
}
