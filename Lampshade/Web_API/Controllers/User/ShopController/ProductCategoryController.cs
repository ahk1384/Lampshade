using _01_LampshadeQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers.User.ShopController;

[ApiController]
[Route("api/shop/ProductCategory/")]
public class ProductCategoryController : ControllerBase
{
    private readonly IProductCategoryQuery _productCategoryQuery;

    public ProductCategoryController(IProductCategoryQuery productCategoryQuery)
    {
        _productCategoryQuery = productCategoryQuery;
    }

    [HttpGet]
    public List<ProductCategoryQueryModel> GetProductCategories()
    {
        return _productCategoryQuery.GetProductCategories();
    }

    [HttpGet("{slug}")]
    public ProductCategoryQueryModel GetProductCategoryWithProducts(string slug)
    {
        return _productCategoryQuery.GetProductCategoryWithProducstsBy(slug);
    }

    [HttpGet("All")]
    public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
    {
        return _productCategoryQuery.GetProductCategoriesWithProducts();
    }
}