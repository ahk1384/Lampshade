using System.Net;
using System.Security.Claims;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.Cart;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ServiceHost.ViewComponent;
using ShopManagementDomain.CartAgg;

namespace ServiceHost.Pages;

public class LoginModel : PageModel
{
    private readonly IAccountApplication _accountApplication;
    private readonly ICartQuery _cartQuery;
    private readonly IAuthHelper _authHelper;
    private readonly ICookieManager _cookieManager;
    public const string CookieName = "cart-items";

    public LoginModel(IAccountApplication accountApplication, ICartQuery cartQuery, IAuthHelper authHelper,
        ICookieManager cookieManager)
    {
        _accountApplication = accountApplication;
        _cartQuery = cartQuery;
        _authHelper = authHelper;
        _cookieManager = cookieManager;
    }

    [TempData] public string LoginMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostLogin(Login command)
    {
        var result = await _accountApplication.Login(command);
        if (result.IsSuccess)
            return RedirectToPage("/Login", "MergeCookies");

        LoginMessage = result.Message;
        return RedirectToPage("/Login");
    }

    public IActionResult OnGetMergeCookies()
    {
        if (_authHelper.IsAuthenticated())
        {
            _cartQuery.AddAllToCart(_cookieManager.GetCartItems(Request),_authHelper.CurrentAccountInfo().Id);
            _cookieManager.Merge(HttpContext.Response);
        }

        return RedirectToPage("/Index");
    }

    public IActionResult OnGetLogout()
    {
        Response.Cookies.Delete(CookieName);
        _accountApplication.Logout();
        return RedirectToPage("/Index");
    }
}