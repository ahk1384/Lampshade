
//using ShopManagement.Application.Contracts.Order;

using _01_LampshadeQuery.Contracts.Cart;
using ShopManagement.Application.Contracts.Cart;

namespace _01_LampshadeQuery.Contracts.Product;

public interface IProductQuery
{
    ProductQueryModel GetProductDetails(string slug);
    List<ProductQueryModel> GetLatestArrivals();

    List<ProductQueryModel> Search(string value);
    
    List<CartItemViewModel> CheckInventoryStatus(List<CartItemViewModel> cartItems);
}