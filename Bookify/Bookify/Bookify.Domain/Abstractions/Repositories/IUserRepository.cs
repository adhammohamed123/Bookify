using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Bookify.Domain.Review;
using Bookify.Domain.User;
using System.Linq.Expressions;

namespace Bookify.Domain.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel?> GetUserAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IQueryable<UserModel>> GetUsersAsync(CancellationToken cancellationToken = default); 
    }
    public interface IApartmentRepository
    {
        Task<ApartmentModel> GetApartmentAsync(Guid apartmentId, CancellationToken cancellationToken = default);
        Task<IQueryable<ApartmentModel>> GetApartmentsAsync(CancellationToken cancellationToken = default);

    }
    public interface IReviewRepository
    {
        Task<ReviewModel> GetReviewAsync(Guid reviewId, CancellationToken cancellationToken = default);
        Task<IQueryable<ReviewModel>> GetReviewsAsync(CancellationToken cancellationToken = default);
    }
    public interface IBookingRepository
    {
        Task<BookingModel> GetBookingAsync(Guid bookingId, CancellationToken cancellationToken = default);
        Task<IQueryable<BookingModel>> GetBookingsAsync(CancellationToken cancellationToken = default);
        Task<bool> IsOverLappedBooking(Guid apartmentId, DateTime startDateUtc, DateTime endDateUtc, CancellationToken cancellationToken = default);

        Task AddBookingAsync(BookingModel booking, CancellationToken cancellationToken = default);
    }

    public interface IBaseRepository<T>
        where T :Entity
    {
        Task<IQueryable<T>> FindAllAsync (bool trackChanges,CancellationToken cancellationToken);
        Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges,CancellationToken cancellationToken);
        Task AddAsync(T entity);
        void Update(T entity);
        public void Remove(T entity)=> entity.Delete();
    }
  


    public interface IRepositoryManager
    {
        public IUserRepository UserRepository { get;}
        public IApartmentRepository ApartmentRepository { get; }
        public IBookingRepository BookingRepository { get; }
        public IReviewRepository ReviewRepository { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); // made as unit of work
    }
}
