using ProEShop.Entities;
using ProEShop.ViewModels.Guarantees;

namespace ProEShop.Services.Contracts;

public interface IGuaranteeService : IGenericService<Guarantee>
{
    Task<ShowGuaranteesViewModel> GetGuarantees(ShowGuaranteesViewModel model);

    Task<EditGuaranteeViewMode?> GetForEdit(long id);
}