using Microsoft.AspNetCore.Mvc;

namespace ProEShop.Web.Areas.Identity.Controllers;
[Area("Identity")]
public class RegisterLoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
