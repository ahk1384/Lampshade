using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Adminstrator.Pages.Accounts.Role;

public class CreateModel : PageModel
{
    private readonly IRoleApplication _roleApplication;
    public CreateRole Command;

    public CreateModel(IRoleApplication roleApplication)
    {
        _roleApplication = roleApplication;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost(CreateRole command)
    {
        var result = _roleApplication.Create(command);
        return RedirectToPage("Index");
    }
}