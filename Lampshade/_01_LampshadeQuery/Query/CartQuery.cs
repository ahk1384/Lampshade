using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Cart;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Cart;
using ShopManagementDomain.CartAgg;
using SM.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class CartQuery : ICartQuery
{
    private readonly ICartService _cartService;
    private readonly ShopContext _shopContext;

    public CartQuery(ShopContext shopContext, ICartCalculatorService cartCalculatorService, ICartService cartService)
    {
        _shopContext = shopContext;
        _cartService = cartService;
    }


    public CartViewModel GetCart(long accountId)
    {
        return _shopContext.Carts.Where(x => !x.IsDeleted).Select(x => new CartViewModel
        {
            CartId = x.Id,
            AccountId = x.AccountId,
            TotalAmount = x.TotalAmount,
            DiscountAmount = x.DiscountAmount,
            PayAmount = x.PayAmount,
            Items = MapItems(x.Items)
        }).AsNoTracking().FirstOrDefault(x => x.AccountId == accountId);
    }

    public OperationResult ChangeItemCount(CartItemViewModel item, long accountId)
    {
        var operationResult = new OperationResult();
        var cart = _shopContext.Carts.FirstOrDefault(x => x.AccountId == accountId && !x.IsDeleted);

        if (cart != null)
        {
            if (cart.Items.Any(x => x.ProductId == item.ProductId) && item.Count > 0)
                cart.Items.FirstOrDefault(x => x.ProductId == item.ProductId).SetCount(item.Count);
            else if (item.Count == 0) RemoveFromCart(item.ProductId, accountId);
            _shopContext.SaveChanges();
            return operationResult.Success();
        }

        return operationResult.Fail();
    }

    public OperationResult AddToCart(CartItemViewModel item, long accountId)
    {
        var operationResult = new OperationResult();
        var cart = _shopContext.Carts.FirstOrDefault(x => x.AccountId == accountId && !x.IsDeleted);

        if (cart != null)
        {
            if (cart.Items.Any(x => x.ProductId == item.ProductId))
            {
                cart.Items.FirstOrDefault(x => x.ProductId == item.ProductId).AddCount(item.Count);
            }
            else
            {
                var cartItem = new CartItem(cart.Id, item.ProductId, item.Name, item.UnitPrice, item.Picture,
                    item.Count, item.IsInStock, item.DiscountRate, item.ProductSlug);
                cart.Items.Add(cartItem);
            }

            _shopContext.SaveChanges();
            return operationResult.Success();
        }

        {
            var c = new Cart(accountId);
            var cartItem = new CartItem(c.Id, item.ProductId, item.Name, item.UnitPrice, item.Picture, item.Count,
                item.IsInStock, item.DiscountRate, item.ProductSlug);
            c.Items.Add(cartItem);
            c.CalculatePayment();
            _shopContext.Carts.Add(c);
            _shopContext.SaveChanges();
            return operationResult.Success();
        }

        return operationResult.Fail();
    }

    public OperationResult AddAllToCart(List<CartItemViewModel> items, long accountId)
    {
        foreach (var item in items)
            if (!AddToCart(item, accountId).IsSuccess)
                return new OperationResult().Fail();

        return new OperationResult().Success();
    }

    public OperationResult RemoveFromCart(long productId, long accountId)
    {
        var operationResult = new OperationResult();
        var cart = _shopContext.Carts.Include(x => x.Items)
            .FirstOrDefault(x => x.AccountId == accountId && !x.IsDeleted);
        if (cart != null && cart.Items.Any(x => x.ProductId == productId))
        {
            cart.Items.Remove(cart.Items.FirstOrDefault(x => x.ProductId == productId));
            _shopContext.SaveChanges();
            return operationResult.Success();
        }

        return operationResult.Fail();
    }

    public OperationResult RemoveCart(long accountId)
    {
        var operationResult = new OperationResult();
        var cart = _shopContext.Carts.FirstOrDefault(x => x.AccountId == accountId && !x.IsDeleted);
        if (cart != null)
        {
            cart.Remove();
            _shopContext.SaveChanges();
            return operationResult.Success();
        }

        return operationResult.Fail();
    }

    private static List<CartItemViewModel> MapItems(List<CartItem> x)
    {
        return x.Select(w =>
                new CartItemViewModel(w.ProductId, w.Name, w.UnitPrice, w.Picture, w.Count, w.IsInStock,
                    w.DiscountRate, w.ProductSlug))
            .ToList();
    }
}