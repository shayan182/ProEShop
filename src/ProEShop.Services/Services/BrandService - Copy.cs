using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;


namespace ProEShop.Services.Services;

public class ProductService : GenericService<Product>, IProductService
{
    private readonly DbSet<Product> _brands;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork uow, IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _brands = uow.Set<Product>();
    }

   
}
