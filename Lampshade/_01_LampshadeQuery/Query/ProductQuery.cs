using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Cart;
using _01_LampshadeQuery.Contracts.Comment;
using _01_LampshadeQuery.Contracts.Product;
using CommentManagement.Infrastructure.EFCore;
using CommnetManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Domain.InventoryAgg;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Cart;
using ShopManagementDomain.CartAgg;
using ShopManagementDomain.ProductPictureAgg;
using SM.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ProductQuery : IProductQuery
{
    private readonly CommentContext _commentContext;
    private readonly ShopContext _context;
    private readonly DiscountContext _discountContext;
    private readonly InventoryContext _inventoryContext;

    public ProductQuery(ShopContext context, DiscountContext discountContext, InventoryContext inventoryContext,
        CommentContext commentContext)
    {
        _context = context;
        _discountContext = discountContext;
        _inventoryContext = inventoryContext;
        _commentContext = commentContext;
    }

    public ProductQueryModel GetProductDetails(string slug)
    {
        var inventory = _inventoryContext.Inventories.Select(x =>
            new { x.ProductId, x.UnitPrice, x.InStock }).ToList();
        var discounts = _discountContext.CustomerDiscounts
            .Where(x => x.StartDate < DateTime.Now && x.EndDate > DateTime.Now)
            .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

        var product = _context.Products
            .Include(a => a.Category).Select(product => new ProductQueryModel
            {
                Id = product.Id,
                Category = product.Category.Title,
                Name = product.Name,
                Code = product.Code,
                Picture = product.Picture,
                PictureAlt = product.PictureAlt,
                PictureTitle = product.PictureTitle,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                Keywords = product.Keywords,
                MetaDescription = product.MetaDescription,
                CategorySlug = product.Category.Slug,
                Pictures = MapProductPicture(product.ProductPictures),
                Slug = product.Slug
            }).AsNoTracking().FirstOrDefault(x => x.Slug == slug);


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

        product.Comments = _commentContext.Comments
            .Where(x => !x.IsDeleted)
            .Where(x => x.IsConfirmed)
            .Where(x => x.Type == CommentType.Product)
            .Where(x => x.OwnerRecordId == product.Id)
            .Select(x => new CommentQueryModel
            {
                Id = x.Id,
                Message = x.Message,
                Name = x.Name,
                Type = x.Type,
                CreationDate = x.CreationDate.ToFarsi()
            })
            .OrderByDescending(x => x.Id)
            .ToList();

        return product;
    }

    public List<ProductQueryModel> GetLatestArrivals()
    {
        var inventory = _inventoryContext.Inventories.Select(x =>
            new { x.ProductId, x.UnitPrice ,x.InStock}).ToList();
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
            }).AsNoTracking().OrderByDescending(x => x.Id).Take(400).ToList();

        foreach (var product in products)
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
                    product.DiscountExpireDate = discount.EndDate.ToDiscountFormat();
                    product.HasDiscount = discountRate > 0;
                    var discountAmount = Math.Round(price * discountRate / 100);
                    product.PriceWithDiscount = (price - discountAmount).ToMoney();
                }
            }
        }

        return products.Where(x => x.IsInStock).Take(8).ToList();
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

    private static List<ProductPictureQueryModel> MapProductPicture(List<ProductPicture> pictures)
    {
        var res = new List<ProductPictureQueryModel>();
        pictures.ForEach(x => res.Add(new ProductPictureQueryModel
        {
            Picture = x.Picture,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            ProductId = x.ProductId,
            IsRemoved = x.IsDeleted
        }));
        return res;
    }
    public List<CartItemViewModel> CheckInventoryStatus(List<CartItemViewModel> cartItems)
    {
        var inventory = _inventoryContext.Inventories.ToList();
        var res = new List<CartItemViewModel>();
        if (cartItems.Count > 0)
        {
            foreach (var cartItem in cartItems.Where(cartItem =>
                         inventory.Any(x => x.ProductId == cartItem.ProductId)))
            {
                var itemInventory = inventory.Find(x => x.ProductId == cartItem.ProductId);
                cartItem.IsInStock = itemInventory.CalculateCurrentCount() >= cartItem.Count;
                res.Add(new CartItemViewModel(cartItem.ProductId,cartItem.Name, cartItem.UnitPrice,cartItem.Picture,cartItem.Count,cartItem.IsInStock,cartItem.DiscountRate));
            }
            
        }
        return res;
    }
}