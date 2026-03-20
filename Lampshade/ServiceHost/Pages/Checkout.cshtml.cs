using System.Globalization;
using _0_Framework.Application;
using _0_Framework.Application.ZarinPal;
using _01_LampshadeQuery;
using _01_LampshadeQuery.Contracts.Cart;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Cart;
using ShopManagement.Application.Contracts.Order;

// using _0_Framework.Application.ZarinPal;

namespace ServiceHost.Pages;

[Authorize]
public class CheckoutModel : PageModel
{
    public const string CookieName = "cart-items";
    private readonly IAuthHelper _authHelper;
    private readonly ICartCalculatorService _cartCalculatorService;
    private readonly ICartQuery _cartQuery;
    private readonly ICartService _cartService;
    private readonly IOrderApplication _orderApplication;
    private readonly IProductQuery _productQuery;

    private readonly IZarinPalFactory _zarinPalFactory;
    public CartViewModel Cart;

    public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService,
        IProductQuery productQuery, IOrderApplication orderApplication,
        IAuthHelper authHelper, IZarinPalFactory zarinPalFactory, ICartQuery cartQuery)
    {
        Cart = new CartViewModel();
        _cartCalculatorService = cartCalculatorService;
        _cartService = cartService;
        _productQuery = productQuery;
        _orderApplication = orderApplication;
        _authHelper = authHelper;
        _zarinPalFactory = zarinPalFactory;
        _cartQuery = cartQuery;
    }

    public void OnGet()
    {
        var serializer = new JavaScriptSerializer();
        var value = Request.Cookies[CookieName];
        var cartItems = serializer.Deserialize<List<CartItemViewModel>>(value);
        foreach (var item in cartItems)
            item.CalculateTotalItemPrice();

        Cart = _cartCalculatorService.ComputeCart(cartItems);
        _cartService.Set(Cart);
    }

    public IActionResult OnPostPay(int paymentMethod)
    {
        var cart = _cartService.Get();
        cart.SetPaymentMethod(paymentMethod);

        var result = _productQuery.CheckInventoryStatus(cart.Items);
        if (result.Any(x => !x.IsInStock))
            return RedirectToPage("/Cart");

        var orderId = _orderApplication.PlaceOrder(cart);
        var userName = _authHelper.CurrentAccountInfo().Fullname;
        var des =
            $"پرداخت سفارش کاربر {orderId} به شماره سفارش {cart.PayAmount} به مبلغ {userName} برای خرید از سایت لمپ شید";
        _cartService.Set(new CartViewModel());
        _cartQuery.RemoveCart(_authHelper.CurrentAccountInfo().Id);
        Response.Cookies.Delete("cart-items");
        if (paymentMethod == 1)
        {
            var paymentResponse = _zarinPalFactory.CreatePaymentRequest(
                cart.PayAmount.ToString(CultureInfo.InvariantCulture), "", "",
                "خرید از درگاه لوازم خانگی و دکوری", orderId);
            return Redirect(
                $"https://sandbox.zarinpal.com/pg/StartPay/{paymentResponse.data.authority}");
        }

        var paymentResult = new PaymentResult();
        return RedirectToPage("/PaymentResult",
            paymentResult.Succeeded(
                "سفارش شما با موفقیت ثبت شد. پس از تماس کارشناسان ما و پرداخت وجه، سفارش ارسال خواهد شد.", null));
    }

    public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status,
        [FromQuery] long oId)
    {
        var orderAmount = _orderApplication.GetAmountBy(oId);
        var verificationResponse =
            _zarinPalFactory.CreateVerificationRequest(authority,
                orderAmount.ToString(CultureInfo.InvariantCulture));

        var result = new PaymentResult();
        if (status == "OK" && verificationResponse.data.code >= 100)
        {
            var issueTrackingNo = _orderApplication.PaymentSucceeded(oId, verificationResponse.data.ref_id);

            result = result.Succeeded("پرداخت با موفقیت انجام شد.", issueTrackingNo);
            return RedirectToPage("/PaymentResult", result);
        }

        result = result.Failed(
            "پرداخت با موفقیت انجام نشد. درصورت کسر وجه از حساب، مبلغ تا 24 ساعت دیگر به حساب شما بازگردانده خواهد شد.");
        return RedirectToPage("/PaymentResult", result);
    }
}