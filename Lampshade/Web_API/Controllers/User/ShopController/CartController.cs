using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Cart;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Application.Contracts.Cart;

namespace Web_API.Controllers.User.ShopController;

[ApiController]
[Route("api/Cart/")]
public class CartController : ControllerBase
{
    private readonly ICartQuery _cartQuery;

    public CartController(ICartQuery cartQuery)
    {
        _cartQuery = cartQuery;
    }

    [HttpGet("{accountId}")]
    public CartViewModel GetCart(long accountId)
    {
        return _cartQuery.GetCart(accountId);
    }

    [HttpPost("AddProducts/{accountId}")]
    public OperationResult AddAllToCart(List<CartItemViewModel> items, long accountId)
    {
        return _cartQuery.AddAllToCart(items, accountId);
    }

    [HttpPost("AddProduct/{accountId}")]
    public OperationResult AddToCart(CartItemViewModel item, long accountId)
    {
        return _cartQuery.AddToCart(item, accountId);
    }

    [HttpPut("UpdateProductCount/{accountId}")]
    public OperationResult ChangeItemCount(CartItemViewModel item, long accountId)
    {
        return _cartQuery.ChangeItemCount(item, accountId);
    }

    [HttpDelete("Remove/{accountId}")]
    public OperationResult RemoveCart(long accountId)
    {
        return _cartQuery.RemoveCart(accountId);
    }

    [HttpDelete("RemoveProduct/{productId}/{accountId}")]
    public OperationResult RemoveFromCart(long productId, long accountId)
    {
        return _cartQuery.RemoveFromCart(productId, accountId);
    }
}