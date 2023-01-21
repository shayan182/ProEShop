using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.ConsignmentItems;

namespace ProEShop.Services.Services;

public class ConsignmentItemService : GenericService<ConsignmentItem>, IConsignmentItemService
{
    private readonly DbSet<ConsignmentItem> _consignmentItems;
    private readonly IMapper _mapper;

    public ConsignmentItemService(
        IUnitOfWork uow,
        IMapper mapper)
        : base(uow)
    {
        _mapper = mapper;
        _consignmentItems = uow.Set<ConsignmentItem>();
    }

    public Task<List<ShowConsignmentItemViewModel>> GetConsignmentItems(long consignmentId)
    {
        return _mapper.ProjectTo<ShowConsignmentItemViewModel>(
            _consignmentItems
        ).Where(x => x.ConsignmentId == consignmentId).ToListAsync();
    }
}