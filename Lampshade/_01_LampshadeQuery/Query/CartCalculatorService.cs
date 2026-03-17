using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Infrastructure.EFCore;
using ShopManagement.Application.Contracts.Cart;

namespace _01_LampshadeQuery.Query;

public class CartCalculatorService : ICartCalculatorService
{
    private readonly IAuthHelper _authHelper;
    private readonly ICartService _cartService;
    private readonly DiscountContext _discountContext;

    public CartCalculatorService(DiscountContext discountContext, IAuthHelper authHelper, ICartService cartService)
    {
        _discountContext = discountContext;
        _authHelper = authHelper;
        _cartService = cartService;
    }

    public CartViewModel ComputeCart(List<CartItemViewModel> cartItems)
    {
        var cart = new CartViewModel();
        var colleagueDiscounts = _discountContext.ColleagueDiscounts
            .Where(x => !x.IsDeleted)
            .Select(x => new { x.DiscountRate, x.ProductId })
            .ToList();

        var customerDiscounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId })
            .ToList();
        var currentAccountRole = _authHelper.CurrentAccountRole();

        foreach (var cartItem in cartItems)
        {
            if (currentAccountRole == Roles.ColleagueUser)
            {
                var colleagueDiscount = colleagueDiscounts.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
                if (colleagueDiscount != null)
                    cartItem.DiscountRate = colleagueDiscount.DiscountRate;
            }
            else
            {
                var customerDiscount = customerDiscounts.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
                if (customerDiscount != null)
                    cartItem.DiscountRate = customerDiscount.DiscountRate;
            }

            cartItem.CalculateTotalItemPrice();
            cartItem.DiscountAmount = cartItem.TotalItemPrice * cartItem.DiscountRate / 100;
            cartItem.ItemPayAmount = cartItem.TotalItemPrice - cartItem.DiscountAmount;
            cart.Add(cartItem);
        }

        return cart;
    }

    // public Cart ComputeCart(List<CartItem> cartItems)
    // {
    //     var cart = _cartService.Get();
    //     var colleagueDiscounts = _discountContext.ColleagueDiscounts
    //         .Where(x => !x.IsDeleted)
    //         .Select(x => new { x.DiscountRate, x.ProductId })
    //         .ToList();
    //
    //     var customerDiscounts = _discountContext.CustomerDiscounts
    //         .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
    //         .Select(x => new { x.DiscountRate, x.ProductId })
    //         .ToList();
    //     var currentAccountRole = _authHelper.CurrentAccountRole();
    //     foreach (var cartItem in cartItems)
    //     {
    //         if (currentAccountRole == Roles.ColleagueUser)
    //         {
    //             var colleagueDiscount = colleagueDiscounts.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
    //             if (colleagueDiscount != null)
    //                 cartItem.SetDiscountRate(colleagueDiscount.DiscountRate);
    //         }
    //         else
    //         {
    //             var customerDiscount = customerDiscounts.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
    //             if (customerDiscount != null)
    //                 cartItem.SetDiscountRate(customerDiscount.DiscountRate);
    //         }
    //
    //         cartItem.CalculateTotalItemPrice();
    //         cart.Add(cartItem);
    //     }
    //
    //     return cart;
    // }

    public CartItemViewModel ComputeCartItem(CartItemViewModel cartItem)
    {
        cartItem.CalculateTotalItemPrice();
        cartItem.DiscountAmount = cartItem.TotalItemPrice * cartItem.DiscountRate / 100;
        cartItem.ItemPayAmount = cartItem.TotalItemPrice - cartItem.DiscountAmount;
        return cartItem;
    }
}