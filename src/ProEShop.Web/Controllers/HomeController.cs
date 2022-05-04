using Microsoft.AspNetCore.Mvc;
using ProEShop.Web.Models;
using System.Diagnostics;
using ProEShop.DataLayer.Context;
using ProEShop.Entities;
using ProEShop.Services.Contracts;

namespace ProEShop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _uow;
    private readonly ICategoryService _categoryService;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork uow, ICategoryService categoryService)
    {
        _logger = logger;
        _uow = uow;
        _categoryService = categoryService;
    }


    public async Task<IActionResult> Index()
    {
        //_categoryService.Add(new Category
        //{
        //    Title = "ك  ي"
        //});
        //await _uow.SaveChangesAsync();
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
