using AutoMapper;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Review.Dtos;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Review.GetReviewQuery
{
    public record GetReivewQuery(Guid id) : ICachedQuery<ReviewDto>
    {
        public string CacheKey => $"Review:{id}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(3);
    }

    internal sealed class GetReviewQueryHandler(IRepositoryManager repositoryManager,IMapper mapper) : IQueryHandler<GetReivewQuery, ReviewDto>
    {
        public async Task<Result<ReviewDto>> Handle(GetReivewQuery request, CancellationToken cancellationToken)
        {
           var review =await repositoryManager.ReviewRepository.GetReviewAsync(request.id, false);
           
            if (review is null)
            {
                return Result.Failure<ReviewDto>(ReviewErrors.NotFound);
            }

            var reviewDto = mapper.Map<ReviewDto>(review);
            return Result.Success(reviewDto);
        }
    }
}