using _0_Framework.Infrastructure;

namespace ShopManagement.Infrastructure.Configuration.Permissions;

public class ShopPermissionsExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "Product", new List<PermissionDto>
                {
                    new(ShopPermissions.ProductList,"Product List") ,
                    new(ShopPermissions.ListProducts, "ListProducts"),
                    new(ShopPermissions.SearchProducts, "SearchProducts"),
                    new(ShopPermissions.CreateProduct, "CreateProduct"),
                    new(ShopPermissions.EditProduct, "EditProduct"),
                    new(ShopPermissions.RemoveAndRestoreProduct , "RemoveAndRestoreProduct"),
                }
            },
            {
                "Product Category", new List<PermissionDto>
                {
                    new(ShopPermissions.ProductCategoryList,"ProductCategory List"),
                    new(ShopPermissions.SearchProductCategories, "SearchProductCategories"),
                    new(ShopPermissions.ListProductCategories, "ListProductCategories"),
                    new(ShopPermissions.CreateProductCategory, "CreateProductCategory"),
                    new(ShopPermissions.EditProductCategory, "EditProductCategory"),
                    new(ShopPermissions.RemoveAndRestoreProductCategory, "RemoveAndRestoreProductCategory"),
                }
            },
            {
                "Product Picture", new List<PermissionDto>
                {
                    new(ShopPermissions.ProductPictureList,"ProductPicture List"),
                    new(ShopPermissions.CreateProductPicture , "CreateProductPicture"),
                    new(ShopPermissions.EditProductPicture ,  "EditProductPicture"),
                    new(ShopPermissions.SearchProductPicture, "SearchProductPicture"),
                    new(ShopPermissions.RemoveAndRestoreProductPicture ,  "RemoveAndRestoreProductPicture")
                }
            },
            {
                "Slide", new List<PermissionDto>
                {
                    new(ShopPermissions.SlideList,"Slide List"),
                    new(ShopPermissions.CreateSlide , "CreateSlide"),
                    new(ShopPermissions.EditSlide ,  "EditSlide"),
                    new(ShopPermissions.SearchSlide ,  "SearchSlide"),
                    new(ShopPermissions.RemoveAndRestoreSlide , "RemoveAndRestoreSlide"),
                }
            },
        };
    }
}