using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application.Contracts.Cart;

namespace Web_API.Controllers.User.ShopController;

[ApiController]
[Route("api/shop/Products/")]
public class ProductController : ControllerBase
{
    private readonly IProductQuery _productQuery;

    public ProductController(IProductQuery productQuery)
    {
        _productQuery = productQuery;
    }

    [HttpGet("{slug}")]
    public ProductQueryModel GetProductDetails(string slug)
    {
        return _productQuery.GetProductDetails(slug);
    }

    [HttpGet("LatestArrivals")]
    public List<ProductQueryModel> GetLatestArrivals()
    {
        return _productQuery.GetLatestArrivals();
    }

    [HttpGet("Search/{value}")]
    public List<ProductQueryModel> Search(string value)
    {
        return _productQuery.Search(value);
    }

    [HttpPost("StockStatus")]
    public List<CartItemViewModel> CheckInventoryStatus(List<CartItemViewModel> cartItems)
    {
        return _productQuery.CheckInventoryStatus(cartItems);
    }
}