using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Cart;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Cart;
using ShopManagementDomain.CartAgg;
using SM.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class CartQuery : ICartQuery
{
    private readonly ShopContext _shopContext;

    public CartQuery(ShopContext shopContext)
    {
        _shopContext = shopContext;
    }


    public CartViewModel GetCart(long accountId)
    {
        return _shopContext.Carts.Select(x => new CartViewModel()
        {
            CartId = x.Id,
            AccountId = x.AccountId,
            TotalAmount = x.TotalAmount,
            DiscountAmount = x.DiscountAmount,
            PayAmount = x.PayAmount,
            Items = MapItems(x.Items)
        }).AsNoTracking().FirstOrDefault(x => x.AccountId == accountId);
    }
    private static List<CartItemViewModel> MapItems(List<CartItem> x)
    {
        return x.Select(w =>
            new CartItemViewModel(w.ProductId,w.Name,w.UnitPrice,w.Picture,w.Count,w.IsInStock,w.DiscountRate))
            .ToList();
    }

    public OperationResult ChangeItemCount(CartItemViewModel item,long accountId)
    {
        var operationResult = new OperationResult();
        var  cart = _shopContext.Carts.FirstOrDefault(x => x.AccountId == accountId);

        if (cart != null)
        {
            if (cart.Items.Any(x => x.ProductId == item.ProductId) && item.Count > 0)
            {
                cart.Items.FirstOrDefault(x => x.ProductId == item.ProductId).SetCount(item.Count);
            }
            else if (item.Count == 0)
            {
                RemoveFromCart(item.ProductId, accountId);
            }
            _shopContext.SaveChanges();
            return operationResult.Success();
        }
        return operationResult.Fail();
    }
    public OperationResult AddToCart(CartItemViewModel item,long accountId)
    {
        var operationResult = new OperationResult();
        var  cart = _shopContext.Carts.FirstOrDefault(x => x.AccountId == accountId);

        if (cart != null)
        {
            if (cart.Items.Any(x => x.ProductId == item.ProductId))
            {
                cart.Items.FirstOrDefault(x => x.ProductId == item.ProductId).AddCount(item.Count);
            }
            else
            {
                CartItem cartItem = new CartItem(cart.Id,item.ProductId,item.Name,item.UnitPrice,item.Picture,item.Count,item.IsInStock,item.DiscountRate);
                cart.Items.Add(cartItem);
            }
            _shopContext.SaveChanges();
            return operationResult.Success();
        }
        else
        {
            Cart c = new Cart(accountId);
            CartItem cartItem = new CartItem(c.Id,item.ProductId,item.Name,item.UnitPrice,item.Picture,item.Count,item.IsInStock,item.DiscountRate);
            c.Items.Add(cartItem);
            _shopContext.Carts.Add(c);
            _shopContext.SaveChanges();
            return operationResult.Success();
        }

        return operationResult.Fail();
    }

    public OperationResult AddAllToCart(List<CartItemViewModel> items, long accountId)
    {
        foreach (var item in items)
        {
            if (!AddToCart(item, accountId).IsSuccess)
                return new OperationResult().Fail();
        }

        return new OperationResult().Success();
    }

    public OperationResult RemoveFromCart(long productId, long accountId)
    {
        var operationResult = new OperationResult();
        var  cart = _shopContext.Carts.Include(x => x.Items).FirstOrDefault(x => x.AccountId == accountId);
        if (cart != null && cart.Items.Any(x => x.ProductId == productId))
        {
            cart.Items.Remove(cart.Items.FirstOrDefault(x => x.ProductId == productId));
            _shopContext.SaveChanges();
            return operationResult.Success();
        }
        else
        {
            return operationResult.Fail();
        }
    }
}