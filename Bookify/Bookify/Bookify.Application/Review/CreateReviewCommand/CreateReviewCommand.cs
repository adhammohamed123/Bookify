using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Review.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookify.Application.Review.CreateReviewCommand
{
    public record CreateReviewCommand : ICommand<ReviewDto>
    {
        public Guid ApartmentId { get; init; }
        public Guid UserId { get; init; }
        public Guid BookingId { get; init; }
        public int Rating { get; init; }
        public string? Comment { get; init; }
    }
}
