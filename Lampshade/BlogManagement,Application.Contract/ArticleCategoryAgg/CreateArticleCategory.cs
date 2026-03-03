using _0_Framework.Application;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BlogManagement_Application.Contract.ArticleCategoryAgg;

public class CreateArticleCategory
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Name { get; set; }

    public IFormFile Picture { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureAlt { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string PictureTitle { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Description { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public int ShowOrder { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Slug { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Keywords { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string MetaDescription { get; set; }

    public string CanonicalAddress { get; set; }
}