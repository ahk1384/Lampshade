using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers.User.AccountsController;

public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}