using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Booking;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastracture.Repositories
{
    internal sealed class BookingRepository(ApplicationDbContext context) : BaseRepository<BookingModel>(context), IBookingRepository
    {
        private static BookingStatus[] NotEmpetyBooking => [BookingStatus.Reserved, BookingStatus.Confirmed, BookingStatus.Completed];
        public async Task AddBookingAsync(BookingModel booking)
        => await AddAsync(booking);
        public async Task<BookingModel?> GetBookingAsync(Guid bookingId, bool trackChanges)
       => await FindByCondition(b => b.Id == bookingId, trackChanges).FirstOrDefaultAsync();
        public IQueryable<BookingModel> GetBookingsAsync(bool trackChanges)
        => FindAll(trackChanges);
        public async Task<bool> IsOverLappedBooking(Guid apartmentId, DateRange duration, CancellationToken cancellationToken = default)
        => await FindByCondition(b => b.ApartmentId == apartmentId && NotEmpetyBooking.Contains(b.Status) &&
            b.Duration.Start <= duration.End && b.Duration.End >= duration.Start, trackChanges: false)
            .AnyAsync(cancellationToken);
    }
}
