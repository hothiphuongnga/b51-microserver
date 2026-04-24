
using AutoMapper;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}