namespace _0_Framework.Application;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class StrongPasswordAttribute : ValidationAttribute
{
    public int MinimumLength { get; set; } = 8;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var password = value as string;

        if (string.IsNullOrEmpty(password))
            return new ValidationResult("Password is required.");

        if (password.Length < MinimumLength)
            return new ValidationResult($"Password must be at least {MinimumLength} characters long.");

        if (!Regex.IsMatch(password, "[A-Z]"))
            return new ValidationResult("Password must contain at least one uppercase letter.");

        if (!Regex.IsMatch(password, "[a-z]"))
            return new ValidationResult("Password must contain at least one lowercase letter.");

        if (!Regex.IsMatch(password, "[0-9]"))
            return new ValidationResult("Password must contain at least one number.");

        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            return new ValidationResult("Password must contain at least one special character.");

        return ValidationResult.Success;
    }
}
