using System.Collections.Generic;
using ShopManagement.Application.Contracts.Cart;

namespace ShopManagement.Application.Contracts.Order;

public interface IOrderApplication
{
    long PlaceOrder(CartViewModel cart);
    double GetAmountBy(long id);
    void Cancel(long id);
    string PaymentSucceeded(long orderId, long refId);
    List<OrderItemViewModel> GetItems(long orderId);
    List<OrderViewModel> Search(OrderSearchModel searchModel);
}