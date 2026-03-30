using _0_Framework.Application;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class RegisterModel : PageModel
{
    private readonly IAccountApplication _accountApplication;
    private readonly IAuthHelper _authHelper;

    public RegisterModel(IAccountApplication accountApplication, IAuthHelper authHelper)
    {
        _accountApplication = accountApplication;
        _authHelper = authHelper;
    }

    [BindProperty] public CreateAccount command { get; set; }
    [TempData] public string RegisterMessage { get; set; }

    public IActionResult OnGet()
    {
        if (_authHelper.IsAuthenticated())
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }

    public IActionResult OnPostRegister()
    {
        ModelState.Remove("command.Roles");
        if (ModelState.IsValid)
        {
            var result = _accountApplication.Register(command);
            if (result.IsSuccess)
            {
                RegisterMessage = null;
                return RedirectToPage("/Login");
            }

            RegisterMessage = result.Message;
            return Page();
        }
        else
        {
            foreach (var error in ModelState.Values)
            {
                if (error.Errors.Count > 0)
                {
                    RegisterMessage = error.Errors[0].ErrorMessage;
                    return Page();
                }
            }

            return RedirectToPage("/Login");
        }
    }
}