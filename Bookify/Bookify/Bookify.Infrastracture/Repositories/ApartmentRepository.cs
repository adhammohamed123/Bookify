using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.Apartment;
using Bookify.Domain.Booking;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastracture.Repositories
{
    internal sealed class ApartmentRepository(ApplicationDbContext context) : BaseRepository<Domain.Apartment.ApartmentModel>(context), IApartmentRepository
    {
        private static BookingStatus[] NotEmpetyApartment=> [BookingStatus.Reserved,BookingStatus.Confirmed, BookingStatus.Completed];
        public async Task AddNewApartmentAsync(ApartmentModel apartmentModel)
        =>  await AddAsync(apartmentModel);

        public IQueryable<ApartmentModel> GetAllApartmentsAvailableInAsync(DateRange duration, bool trackChanges)
        => FindByCondition(a =>a.Bookings!.Any(b=> !NotEmpetyApartment.Contains(b.Status) && b.Duration.Start <=duration.End && b.Duration.End >=duration.Start ),trackChanges);

        public async Task<ApartmentModel?> GetApartmentAsync(Guid apartmentId, bool trackChanges)
       =>await FindByCondition(a => a.Id == apartmentId, trackChanges).FirstOrDefaultAsync();
        public IQueryable<ApartmentModel> GetApartmentsAsync(bool trackChanges)
        => FindAll(trackChanges);
    }
}
