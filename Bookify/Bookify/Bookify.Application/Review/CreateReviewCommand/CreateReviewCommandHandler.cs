using AutoMapper;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Review.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Bookify.Domain.User;

namespace Bookify.Application.Review.CreateReviewCommand
{
    public class CreateReviewCommandHandler(IRepositoryManager repositoryManager,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper) : ICommandHandler<CreateReviewCommand, ReviewDto>
    {
        private readonly IRepositoryManager repositoryManager = repositoryManager;

        public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var user =await repositoryManager.UserRepository.GetUserAsync(request.UserId,false);

            if(user == null)
                 return Result.Failure<ReviewDto>(UserErrors.UserNotFound);

            var apartment = await repositoryManager.ApartmentRepository.GetApartmentAsync(request.ApartmentId,false);

            if(apartment == null)
                 return Result.Failure<ReviewDto>(ApartmentErrors.ApartmentNotFound);

            var booking = await repositoryManager.BookingRepository.GetBookingAsync(request.BookingId,false);
            if(booking == null)
                    return Result.Failure<ReviewDto>(BookingErrors.BookingNotFound);

            var ratingResult = Domain.Review.Rating.Create(request.Rating); 
           var comment = new Domain.Review.Comment(request.Comment ?? string.Empty);

            var reviewResult = Domain.Review.ReviewModel.Create(request.UserId,request.ApartmentId,booking,ratingResult,comment,dateTimeProvider.UtcNow);

            if (reviewResult.IsFaliure)
            {
                return Result.Failure<ReviewDto>(reviewResult.Error);
            }
            await repositoryManager.ReviewRepository.AddReviewAsync(reviewResult.Value);
            await repositoryManager.SaveChangesAsync();
            var reviewDto = mapper.Map<ReviewDto>(reviewResult.Value);

            return Result.Success(reviewDto);
        }
    }
}
