using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using _0_Framework.Application;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Http;

namespace AccountManagement.Application.Contract.Account;

public class CreateAccount
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Fullname { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Username { get; set; }
    
    [DataType(DataType.Password)]
    [StrongPassword(ErrorMessage = ValidationMessages.PasswordWeak)]
    public string Password { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Mobile { get; set; }

    public long RoleId { get; set; }

    public List<int>? Permissions { get; set; }
    public IFormFile? ProfilePhoto { get; set; }
    [AllowNull]
    public List<RoleViewModel> Roles { get; set; }
}