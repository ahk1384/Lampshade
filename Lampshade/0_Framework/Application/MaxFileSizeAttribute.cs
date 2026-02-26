    using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace _0_Framework.Application;

public class MaxFileSizeAttribute :ValidationAttribute, IClientModelValidator
{
    private readonly int _maxSize;

    public MaxFileSizeAttribute(int maxSize)
    {
        _maxSize = maxSize;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        //context.Attributes.Add("data-val", "true");
        context.Attributes.Add("data-val-maxFileSize", ErrorMessage);
    }

    public override bool IsValid(object? value)
    {
        var file = value as IFormFile;
        if (file != null) return true;
        return file.Length <= _maxSize;
    }
}