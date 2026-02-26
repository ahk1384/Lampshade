using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Product;
using _01_LampshadeQuery.Contracts.ProductCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagementDomain.ProductAgg;
using SM.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ProductQuery : IProductQuery
{
    private readonly ShopContext _context;
    private readonly DiscountContext _discountContext;
    private readonly InventoryContext _inventoryContext;

    public ProductQuery(ShopContext context, DiscountContext discountContext, InventoryContext inventoryContext)
    {
        _context = context;
        _discountContext = discountContext;
        _inventoryContext = inventoryContext;
    }

    public ProductQueryModel GetProductDetails(string slug)
    {
        throw new NotImplementedException();
    }

    public List<ProductQueryModel> GetLatestArrivals()
    {
        var inventory = _inventoryContext.Inventories.Select(x =>
            new { x.ProductId, x.UnitPrice }).ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

        var products = _context.Products
            .Include(a => a.Category).Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Category = product.Category.Title,
                Name = product.Name,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Slug = product.Slug
            }).AsNoTracking().OrderByDescending(x => x.Id).Take(6).ToList();

        foreach (var product in products)
        {
            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                if (discount != null)
                {
                    var discountRate = discount.DiscountRate;
                    product.DiscountRate = discountRate;
                    product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                    product.HasDiscount = discountRate > 0;
                    var discountAmount = Math.Round(price * discountRate / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }
        }
        return products;
    }

    public List<ProductQueryModel> Search(string value)
    {
        var inventory = _inventoryContext.Inventories.Select(x =>
            new { x.ProductId, x.UnitPrice }).ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

        var products = _context.Products
            .Include(a => a.Category).Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Category = product.Category.Title,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                Slug = product.Slug
            }).AsNoTracking();
        if (!string.IsNullOrWhiteSpace(value))
            products = products.Where(x => x.Name.Contains(value) || x.ShortDescription.Contains(value));
        var finalproducts = products.OrderByDescending(x => x.Id).ToList();

        foreach (var product in finalproducts)
        {
            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.Price = price.ToMoney();
                var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                if (discount != null)
                {
                    var discountRate = discount.DiscountRate;
                    product.DiscountRate = discountRate;
                    product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                    product.HasDiscount = discountRate > 0;
                    var discountAmount = Math.Round(price * discountRate / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }
        }

        return finalproducts;
    }
}