using _0_Framework.Application;
using _0_Framework.Application.Sms;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Cart;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.Services;
using ShopManagementDomain.OrderAgg;
using ShopManagementDomain.Services;

namespace ShopManagement.Application;

public class OrderApplication : IOrderApplication
{
    private readonly IAuthHelper _authHelper;
    private readonly IConfiguration _configuration;
    private readonly IOrderRepository _orderRepository;
    private readonly IShopAccountAcl _shopAccountAcl;
    private readonly IShopInventoryAcl _shopInventoryAcl;

    private readonly ISmsService _smsService;
    // private readonly ISmsService _smsService;

    public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration,
        IShopInventoryAcl shopInventoryAcl, IShopAccountAcl shopAccountAcl, ISmsService smsService)
    {
        _orderRepository = orderRepository;
        _authHelper = authHelper;
        _configuration = configuration;
        _shopInventoryAcl = shopInventoryAcl;
        _shopAccountAcl = shopAccountAcl;
        _smsService = smsService;
    }

    public long PlaceOrder(CartViewModel cart)
    {
        _orderRepository.BeginTran();
        var currentAccountId = _authHelper.CurrentAccountInfo().Id;


        var order = new Order(currentAccountId, cart.PaymentMethod, cart.TotalAmount, cart.DiscountAmount,
            cart.PayAmount);

        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem(cartItem.ProductId, cartItem.Count, cartItem.UnitPrice,
                cartItem.DiscountRate);
            order.AddItem(orderItem);
        }

        _orderRepository.Create(order);
        if (!_shopInventoryAcl.ReduceFromInventory(order.Items))
            return 0;
        _orderRepository.CommitTran();
        return order.Id;
    }


    public double GetAmountBy(long id)
    {
        return _orderRepository.GetAmountBy(id);
    }

    public void Cancel(long id)
    {
        _orderRepository.BeginTran();
        var order = _orderRepository.Get(id);
        order.Cancel();
        _orderRepository.CommitTran();
    }

    public string PaymentSucceeded(long orderId, long refId)
    {
        _orderRepository.BeginTran();
        var order = _orderRepository.Get(orderId);
        order.PaymentSucceeded(refId);
        order.Confirm();
        var symbol = _configuration.GetSection("Symbol").Value;
        var issueTrackingNo = CodeGenerator.Generate(symbol);
        order.SetIssueTrackingNo(issueTrackingNo);
        order.Restore();
        if (!_shopInventoryAcl.ReduceFromInventory(order.Items)) return "";

        _orderRepository.CommitTran();

        var (name, mobile) = _shopAccountAcl.GetAccountBy(order.AccountId);

        // _smsService.Send(mobile,
        //     $"{name} گرامی سفارش شما با شماره پیگیری {issueTrackingNo} با موفقیت پرداخت شد و ارسال خواهد شد.");
        return issueTrackingNo;
    }

    public List<OrderItemViewModel> GetItems(long orderId)
    {
        return _orderRepository.GetItems(orderId);
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        return _orderRepository.Search(searchModel);
    }
}