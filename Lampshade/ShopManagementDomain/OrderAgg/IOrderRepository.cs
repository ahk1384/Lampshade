using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.Order;

namespace ShopManagementDomain.OrderAgg;

public interface IOrderRepository : IRepository<long, Order>
{
    double GetAmountBy(long id);
    List<OrderItemViewModel> GetItems(long orderId);
    List<OrderViewModel> Search(OrderSearchModel searchModel);
}