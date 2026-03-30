using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers.AccountsController;

public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}