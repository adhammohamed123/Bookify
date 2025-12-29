using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Bookify.Domain.Review;
using Bookify.Domain.User;
using System.Linq.Expressions;

namespace Bookify.Domain.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task AddNewUser(UserModel newUser);
        Task<UserModel?> GetUserAsync(Guid userId, bool trackChanges);
        IQueryable<UserModel> GetUsersAsync(bool trackChanges); 
    }
    public interface IApartmentRepository
    {
        Task AddNewApartmentAsync(ApartmentModel apartmentModel);
        Task<ApartmentModel?> GetApartmentAsync(Guid apartmentId, bool trackChanges);
        IQueryable<ApartmentModel> GetApartmentsAsync(bool trackChanges);
       IQueryable<ApartmentModel> GetAllApartmentsAvailableInAsync(DateRange duration, bool trackChanges);

    }
    public interface IReviewRepository
    {
        Task<ReviewModel?> GetReviewAsync(Guid reviewId, bool trackChanges);
        IQueryable<ReviewModel> GetReviewsAsync(bool trackChanges);
    }
    public interface IBookingRepository
    {
        Task<BookingModel?> GetBookingAsync(Guid bookingId, bool trackChanges);
        IQueryable<BookingModel> GetBookingsAsync(bool trackChanges);
        Task<bool> IsOverLappedBooking(Guid apartmentId, DateRange duration, CancellationToken cancellationToken = default);

        Task AddBookingAsync(BookingModel booking);
    }

    public interface IBaseRepository<T>
        where T :Entity
    {
        IQueryable<T> FindAll (bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
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
