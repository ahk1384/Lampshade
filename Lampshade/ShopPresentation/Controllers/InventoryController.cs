using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application.Contracts.ProductAgg;

namespace ShopPresentation.Controllers;
[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IProductApplication _productApplication;

    public InventoryController(IProductApplication productApplication)
    {
        _productApplication = productApplication;
    }

    [HttpGet]
    public List<ProductViewModel> Products()
    {
        return _productApplication.GetProducts();
    }
}