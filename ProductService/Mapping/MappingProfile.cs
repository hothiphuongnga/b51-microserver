
using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}