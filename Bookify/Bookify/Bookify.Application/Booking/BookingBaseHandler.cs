using AutoMapper;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Bookify.Domain.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking
{
    internal abstract class BookingBaseHandler(IRepositoryManager repositoryManager)
    {
        protected readonly IRepositoryManager repositoryManager = repositoryManager;
        

        protected async Task<Result<UserModel>> CheckUserExistanceAsync(Guid UserId)
        {
            var user = await repositoryManager.UserRepository.GetUserAsync(UserId);
            if (user is null)
            {
                return Result.Failure<UserModel>(UserErrors.UserNotFound);
            }
            return Result.Success(user);
        }

        protected async Task<Result<ApartmentModel>> CheckApartmentExistanceAsync(Guid ApartmentId)
        {
            var apartment = await repositoryManager.ApartmentRepository.GetApartmentAsync(ApartmentId);
            if (apartment is null)
            {
                return Result.Failure<ApartmentModel>(ApartmentErrors.ApartmentNotFound);
            }
            return Result.Success(apartment);
        }
        protected async Task<Result<BookingModel>> CheckBookingExistance(Guid BookingId)
        {
            var booking = await repositoryManager.BookingRepository.GetBookingAsync(BookingId);
            if (booking is null)
            {
                return Result.Failure<BookingModel>(BookingErrors.BookingNotFound);
            }
            return Result.Success(booking);

        }

      
        
    }
}
