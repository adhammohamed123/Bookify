using Bookify.Domain.Abstractions;
using Bookify.Domain.Booking;
using Bookify.Domain.Review.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Domain.Review
{
    public sealed class ReviewModel : Entity
    {
        private ReviewModel(
            Guid id,
            Guid userId, 
            Guid apartmentId, 
            Guid bookingId, 
            Rating rating, 
            Comment comment, 
            DateTime createdAtUtc)
            :base(id)
        {
            UserId = userId;
            ApartmentId = apartmentId;
            BookingId = bookingId;
            Rating = rating;
            Comment = comment;
            CreatedAtUtc = createdAtUtc;
        }

       
        public Guid UserId { get; private set; }
        public Guid ApartmentId { get;private set; }
        public Guid BookingId { get;private set; }

        public Rating Rating { get; private set; }
        public Comment Comment { get; private set; }
        public DateTime CreatedAtUtc { get;private set; }



        public static Result<ReviewModel> Create(
            Guid userId,
            Guid apartmentId,
            Booking.BookingModel booking,
            Rating rating,
            Comment comment,
            DateTime createdAtUtc)
        {

            if(booking.Status != BookingStatus.Completed)
            {
                return Result.Failure<ReviewModel>(ReviewErrors.BookingNotCompleted);
            }


            var review = new ReviewModel(
                Guid.NewGuid(),
                userId,
                apartmentId,
                booking.Id,
                rating,
                comment,
                createdAtUtc);

            review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

            return Result.Success(review);
        }
    }
}
