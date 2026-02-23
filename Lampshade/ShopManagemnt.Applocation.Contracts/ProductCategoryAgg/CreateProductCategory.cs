using _0_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace ShopManagement.Application.Contracts.ProductCategoryAgg;

public class CreateProductCategory
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Title { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Description { get; set; } = string.Empty;

    public string CreationDate { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Picture { get; set; }

    public string PictureAlt { get; set; }

    public string PictureTitle { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string MetaDescription { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Keywords { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Slug { get; set; }
}