using Bookify.Domain.Abstractions.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastracture.Repositories
{
    public sealed class RepositoryManager(ApplicationDbContext context, IPublisher publisher) : IRepositoryManager
    {
        private readonly ApplicationDbContext context = context;
        private readonly IPublisher publisher = publisher;
        private readonly Lazy<IUserRepository> userRepository = new(() => new UserRepository(context));
        private readonly Lazy<IApartmentRepository> apartmentRepository = new(() => new ApartmentRepository(context));
        private readonly Lazy<IBookingRepository> bookingRepository = new(() => new BookingRepository(context));
        private readonly Lazy<IReviewRepository> reviewRepository = new(() => new ReviewRepository(context));


        public IUserRepository UserRepository => userRepository.Value;
        public IApartmentRepository ApartmentRepository => apartmentRepository.Value;
        public IBookingRepository BookingRepository => bookingRepository.Value;
        public IReviewRepository ReviewRepository => reviewRepository.Value;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await context.SaveChangesAsync(cancellationToken);
                await PublishDomainEvents(cancellationToken);
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new 
            }

        }
        private async Task PublishDomainEvents(CancellationToken cancellationToken)
        {
            var domainEvents = context.GetTrackedEntities().SelectMany(e =>
            {
                var events = e.GetDomainEvents;
                e.ClearDomainEvents();
                return events;
            }).ToList();

            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
