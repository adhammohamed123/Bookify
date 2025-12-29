using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Repositories;
using Bookify.Domain.User;

namespace Bookify.Application.User.Register
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
    {
        private readonly IRepositoryManager repositoryManager;

        // ctor injection of required dependencies

        public CreateUserCommandHandler(
         IRepositoryManager repositoryManager
         )
        {
            this.repositoryManager = repositoryManager;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user=await repositoryManager.UserRepository.GetUserAsync(request.Id,trackChanges:true);

            if (user is null)
            {
                var NewUser = UserModel.CreateNewUser(request.Id, new FirstName(request.FirestName), new LastName(request.LastName), Email.Create(request.Email));
                await repositoryManager.UserRepository.AddNewUser(NewUser);
                await repositoryManager.SaveChangesAsync(cancellationToken);
                return NewUser.Value.Id;
            }

            if(user!.FirstName.Value != request.FirestName || user.LastName.Value != request.LastName || user.Email.Value != request.Email)
            {
                user.UpdateData(new FirstName(request.FirestName),new LastName(request.LastName),Email.Create(request.Email));
                await repositoryManager.SaveChangesAsync(cancellationToken);
            }
            
            return user.Id;
        }
        
    }
}
