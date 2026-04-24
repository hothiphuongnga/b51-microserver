using UserService.Data;
using UserService.Models;
using UserService.Repositories.Base;

namespace UserService.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    
}

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(UserDbServiceContext context) : base(context)
    {
        
    }
}