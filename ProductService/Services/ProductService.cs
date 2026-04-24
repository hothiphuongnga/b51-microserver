using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;
using ProductService.Repositories;
using ProductService.Repositories.Base;
using ProductService.Services.Base;

namespace UserService.Services;

public interface IProductService : IServiceBase<Product, ProductDto>
{
    Task<ResponseEntity> UpdateStockAsync(int id, int quantity);
}

public class ProductServicee : ServiceBase<Product, ProductDto>, IProductService
{

    private readonly IProductRepository _ProductRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductServicee(IProductRepository ProductRepository, IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper,ProductRepository)
    {
        _ProductRepository = ProductRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseEntity> UpdateStockAsync(int id, int quantity)
    {
        // tìm sp 
        var product = await _ProductRepository.GetByIdAsync(id);

        // kiểm tra tồn kho có đủ để trừ hay không
        if(product.Stock >= quantity)
        {
            product.Stock -= quantity;
            _ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return ResponseEntity.Ok(product);
        }
        else
        {
            return ResponseEntity.Fail("Not enough stock",400);
        }


    }
}