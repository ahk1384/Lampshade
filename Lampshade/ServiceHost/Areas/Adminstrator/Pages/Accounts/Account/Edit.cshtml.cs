using _0_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Adminstrator.Pages.Accounts.Account;

public class EditModel : PageModel
{
    private readonly IEnumerable<IPermissionExposer> _exposers;
    private readonly IRoleApplication _roleApplication;
    private readonly IAccountApplication _accountApplication;
    public List<RoleViewModel> roles { get; set; } = new List<RoleViewModel>();
    public EditAccount Command;
    public List<SelectListItem> Permissions = new();
    public EditModel(IRoleApplication roleApplication, IEnumerable<IPermissionExposer> exposers, IAccountApplication accountApplication)
    {
        _roleApplication = roleApplication;
        _exposers = exposers;
        _accountApplication = accountApplication;
    }

    public void OnGet(long id)
    {
        roles = _roleApplication.List();
        Command = _accountApplication.GetDetails(id);
        var rolePermissions = _roleApplication.GetDetails(Command.RoleId);
        foreach (var exposer in _exposers)
        {
            var exposedPermissions = exposer.Expose();
            foreach (var (key, value) in exposedPermissions)
            {
                var group = new SelectListGroup { Name = key };
                foreach (var permission in value)
                {
                    if (!rolePermissions.MappedPermissions.Any(x => x.Code == permission.Code))
                    {
                        var item = new SelectListItem(permission.Name, permission.Code.ToString())
                        {
                            Group = group
                        };

                        if (Command.MappedPermissions.Any(x => x.Code == permission.Code))
                            item.Selected = true;
                        Permissions.Add(item);
                    }
                }
            }
        }
    }

    public IActionResult OnPost(EditAccount command)
    {
        var result = _accountApplication.Edit(command);
        return RedirectToPage("Index");
    }
}