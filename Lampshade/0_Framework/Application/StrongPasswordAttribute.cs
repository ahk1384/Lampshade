using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace _0_Framework.Application;

public class StrongPasswordAttribute : ValidationAttribute
{
    public int MinimumLength { get; set; } = 8;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var password = value as string;

        if (string.IsNullOrEmpty(password))
            return new ValidationResult("پسورد الزامی است .");

        if (password.Length < MinimumLength)
            return new ValidationResult($"پسورد با حداقل دارای طول {MinimumLength} باشد");

        if (!Regex.IsMatch(password, "[A-Z]"))
            return new ValidationResult("پسورد باید دارای حداقل یک حرف بزرگ انگلیسی باشد .");

        if (!Regex.IsMatch(password, "[a-z]"))
            return new ValidationResult("پسورد باید دارای حداقل یک حرف کوچک انگلیسی باشد .");

        if (!Regex.IsMatch(password, "[0-9]"))
            return new ValidationResult("پسورد باید دارای اعداد باشد .");

        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            return new ValidationResult("پسورد بای حداقل دارای یک علامت خاص باشد .");

        return ValidationResult.Success;
    }
}