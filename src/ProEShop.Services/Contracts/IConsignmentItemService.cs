using ProEShop.Entities;
using ProEShop.ViewModels.ConsignmentItems;

namespace ProEShop.Services.Contracts;

public interface IConsignmentItemService : IGenericService<ConsignmentItem>
{
    Task<List<ShowConsignmentItemViewModel>> GetConsignmentItems(long consignmentId);
   
}