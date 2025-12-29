using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.User;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bookify.Infrastracture.Repositories
{
    internal sealed class UserRepository(ApplicationDbContext context) : BaseRepository<UserModel>(context), IUserRepository
    {
        public async Task AddNewUser(UserModel newUser)
       => await AddAsync(newUser);

        public async Task<UserModel?> GetUserAsync(Guid userId, bool trackChanges)
        =>await FindByCondition(u=>u.Id==userId, trackChanges).FirstOrDefaultAsync();

        public IQueryable<UserModel> GetUsersAsync(bool trackChanges)
        => FindAll(trackChanges);
    }
}
