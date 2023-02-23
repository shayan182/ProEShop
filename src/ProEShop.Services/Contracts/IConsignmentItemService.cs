using ProEShop.Entities;
using ProEShop.ViewModels.Consignments;

namespace ProEShop.Services.Contracts;

public interface IConsignmentItemService : IGenericService<ConsignmentItem>
{
    Task<bool> IsExistsByProductVariantIdAndConsignmentId(long productVariantId, long consignmentId);

}