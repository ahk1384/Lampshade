using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages;

public class RegisterModel : PageModel
{
    private readonly IAccountApplication _accountApplication;
    [BindProperty]public CreateAccount command {get; set; }

    public RegisterModel(IAccountApplication accountApplication)
    {
        _accountApplication = accountApplication;
    }
    [TempData] public string RegisterMessage { get; set; }

    public void OnGet()
    { }

    public IActionResult OnPostRegister()
    {
        ModelState.Remove("command.Roles");
        if (ModelState.IsValid)
        {
            var result = _accountApplication.Register(command);
            if (result.IsSuccess)
                return RedirectToPage("/Login");
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