using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProEShop.Common.Constants;
using ProEShop.Common.Helpers;
using ProEShop.Common.IdentityToolkit;
using ProEShop.DataLayer.Context;
using ProEShop.Services.Contracts;
using ProEShop.ViewModels.Guarantees;
using ProEShop.ViewModels.Variants;

namespace ProEShop.Web.Pages.Admin.Guarantee;

public class IndexModel : PageBase
{
    #region Constructor

    private readonly IGuaranteeService _guaranteeService;
    private readonly IMapper _mapper;
    private IUnitOfWork _uow;
    private IUploadFileService _uploadFileService;

    public IndexModel(
        IGuaranteeService guaranteeService, IMapper mapper, IUnitOfWork uow, IUploadFileService uploadFileService)
    {
        _guaranteeService = guaranteeService;
        _mapper = mapper;
        _uow = uow;
        _uploadFileService = uploadFileService;
    }

    #endregion

    [BindProperty(SupportsGet = true)]
    public ShowGuaranteesViewModel Guarantees { get; set; }
        = new();

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnGetGetDataTableAsync()
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }
        return Partial("List", await _guaranteeService.GetGuarantees(Guarantees));
    }

    public IActionResult OnGetAdd()
    {
        return Partial("Add");
    }

    public async Task<IActionResult> OnPostAdd(AddGuaranteeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.ModelStateErrorMessage)
            {
                Data = ModelState.GetModelStateErrors()
            });
        }

        var guarantee = _mapper.Map<Entities.Guarantee>(model);
        guarantee.IsConfirmed = true;

        string pictureFileName = null;
        if (model.Picture.IsFileUploaded())
            pictureFileName = model.Picture.GenerateFileName();
        guarantee.Picture = pictureFileName;


        await _guaranteeService.AddAsync(guarantee);
        await _uow.SaveChangesAsync();
        await _uploadFileService.SaveFile(model.Picture, guarantee.Picture, null, "images", "guarantees");
        return Json(new JsonResultOperation(true, " گرانتی مورد نظر با موفقیت اضافه شد"));
    }


    public async Task<IActionResult> OnGetEdit(long id)
    {
        var model = await _guaranteeService.GetForEdit(id);
        if (model is null)
        {
            return Json(new JsonResultOperation(false, PublicConstantStrings.RecordNotFoundMessage));
        }

        return Partial("Edit", model);
    }
    
}