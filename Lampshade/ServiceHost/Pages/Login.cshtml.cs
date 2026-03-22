using _0_Framework.Application;
using _0_Framework.Application.Sms;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.Cart;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class LoginModel : PageModel
{
    public const string CookieName = "cart-items";
    private readonly IAccountApplication _accountApplication;
    private readonly IAuthHelper _authHelper;
    private readonly ICartQuery _cartQuery;
    private readonly ICookieManager _cookieManager; 
    [BindProperty] public Login command {get; set; }

    public LoginModel(IAccountApplication accountApplication, ICartQuery cartQuery, IAuthHelper authHelper,
        ICookieManager cookieManager, ISmsService smsService)
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

    public async Task<IActionResult> OnPostLogin()
    {
        var result = await _accountApplication.Login(command);
        if (result.IsSuccess)
            return RedirectToPage("/Login", "MergeCookies");

        LoginMessage = result.Message;
        return Page();
    }

    public IActionResult OnGetMergeCookies()
    {
        if (_authHelper.IsAuthenticated())
        {
            _cartQuery.AddAllToCart(_cookieManager.GetCartItems(Request), _authHelper.CurrentAccountInfo().Id);
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