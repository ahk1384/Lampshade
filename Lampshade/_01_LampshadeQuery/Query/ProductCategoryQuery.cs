using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Product;
using _01_LampshadeQuery.Contracts.ProductCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagementDomain.ProductAgg;
using SM.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ProductCategoryQuery : IProductCategoryQuery
{
    private readonly ShopContext _context;
    private readonly DiscountContext _discountContext;
    private readonly InventoryContext _inventoryContext;

    public ProductCategoryQuery(ShopContext context, DiscountContext discountContext, InventoryContext inventoryContext)
    {
        _context = context;
        _discountContext = discountContext;
        _inventoryContext = inventoryContext;
    }

    public ProductCategoryQueryModel GetProductCategoryWithProducstsBy(string slug)
    {
        var inventory = _inventoryContext.Inventories.Select(x =>
            new { x.ProductId, x.UnitPrice ,x.InStock}).ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

        var catetory = _context.ProductCategories
            .Include(a => a.Products)
            .ThenInclude(x => x.Category)
            .Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Title,
                Description = x.Description,
                MetaDescription = x.MetaDescription,
                Keywords = x.Keywords,
                Slug = x.Slug,
                Products = MapProducts(x.Products)
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);

        foreach (var product in catetory.Products)
        {
            var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
            if (productInventory != null)
            {
                var price = productInventory.UnitPrice;
                product.DoublePrice = price;
                product.Price = price.ToMoney();
                product.IsInStock = productInventory.InStock;
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

        return catetory;
    }

    public List<ProductCategoryQueryModel> GetProductCategories()
    {
        return _context.ProductCategories.Include(x => x.Products).ThenInclude(x => x.Category).Select(x =>
            new ProductCategoryQueryModel
            {
                Id = x.Id,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Name = x.Title,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Products = MapProducts(x.Products),
                Slug = x.Slug
            }).AsNoTracking().ToList();
    }

    public List<ProductCategoryQueryModel> GetProductCategoriesWithProducts()
    {
        var inventory = _inventoryContext.Inventories.Select(x =>
            new { x.ProductId, x.UnitPrice,x.InStock }).ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId }).ToList();
        var categories = _context.ProductCategories.Include(x => x.Products).ThenInclude(x => x.Category).Select(x =>
            new ProductCategoryQueryModel
            {
                Id = x.Id,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.MetaDescription,
                Name = x.Title,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug,
                Products = MapProducts(x.Products)
            }).AsNoTracking().ToList();
        ;
        foreach (var category in categories)
        {
            foreach (var product in category.Products)
            {
                var productInventory = inventory.FirstOrDefault(x => x.ProductId == product.Id);
                if (productInventory != null)
                {
                    var price = productInventory.UnitPrice;
                    product.Price = price.ToMoney();
                    product.IsInStock = productInventory.InStock;
                    var discount = discounts.FirstOrDefault(x => x.ProductId == product.Id);
                    if (discount != null)
                    {
                        var discountRate = discount.DiscountRate;
                        product.DiscountRate = discountRate;
                        product.HasDiscount = discountRate > 0;
                        var discountAmount = Math.Round(price * discountRate / 100);
                        product.PriceWithDiscount = (price - discountAmount).ToMoney();
                    }
                }
            }
            category.Products = category.Products.OrderBy(x => x.IsInStock).ToList();
        }
        

        return categories;
    }

    private static List<ProductQueryModel> MapProducts(List<Product> products)
    {
        var res = new List<ProductQueryModel>();
        products.ForEach(x => res.Add(new ProductQueryModel
        {
            Id = x.Id,
            Category = x.Category.Title,
            Name = x.Name,
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            ShortDescription = x.ShortDescription,
            Slug = x.Slug
        }));
        return res;
    }
}