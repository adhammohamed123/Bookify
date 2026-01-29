using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Review;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastracture.Repositories
{
    internal sealed class ReviewRepository(ApplicationDbContext context) : BaseRepository<ReviewModel>(context), IReviewRepository
    {
        public async Task AddReviewAsync(ReviewModel review)
        =>await AddAsync(review);

        public async Task<ReviewModel?> GetReviewAsync(Guid reviewId, bool trackChanges)
       => await FindByCondition(r => r.Id == reviewId, trackChanges).FirstOrDefaultAsync();
        public IQueryable<ReviewModel> GetReviewsAsync(bool trackChanges)
        => FindAll(trackChanges);
    }
}
