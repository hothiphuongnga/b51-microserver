using AutoMapper;
using UserService.Dtos;
using UserService.Models;
using UserService.Repositories;
using UserService.Repositories.Base;
using UserService.Services.Base;

namespace UserService.Services;

public interface IUserServicee : IServiceBase<User, UserDto>
{
    
}

public class UserServicee : ServiceBase<User, UserDto>, IUserServicee
{

    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserServicee(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper,userRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
}