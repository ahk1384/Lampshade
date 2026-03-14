using _0_Framework.Application;
using _0_Framework.Infrastructure;

namespace ShopManagement.Infrastructure.Configuration.Permissions;

public class ShopPermissions : IPermissions
{
    //Product
    public const int BaseShop = 1000;
    public const int BaseProduct = BaseShop + 100;
    public const int ListProducts = BaseProduct + 10;
    public const int SearchProducts = BaseProduct + 11;
    public const int CreateProduct = BaseProduct + 12;
    public const int EditProduct = BaseProduct + 13;
    public const int RemoveAndRestoreProduct = BaseProduct + 14;

    public const int ProductList = BaseProduct + 15;

    //ProductCategory   
    public const int BaseProductCategory = BaseShop + 200;
    public const int ListProductCategories = BaseProductCategory + 20;
    public const int SearchProductCategories = BaseProductCategory + 21;
    public const int CreateProductCategory = BaseProductCategory + 22;
    public const int EditProductCategory = BaseProductCategory + 23;
    public const int RemoveAndRestoreProductCategory = BaseProductCategory + 24;
    public const int ProductCategoryList = BaseProductCategory + 25;

    //ProductPicture
    public const int BaseProductPicture = BaseShop + 300;
    public const int CreateProductPicture = BaseProductPicture + 30;
    public const int EditProductPicture = BaseProductPicture + 31;
    public const int SearchProductPicture = BaseProductPicture + 32;
    public const int RemoveAndRestoreProductPicture = BaseProductPicture + 33;
    public const int ProductPictureList = BaseProductPicture + 34;

    //Slide
    public const int BaseSlide = BaseShop + 400;
    public const int SearchSlide = BaseSlide + 40;
    public const int CreateSlide = BaseSlide + 41;
    public const int EditSlide = BaseSlide + 42;
    public const int RemoveAndRestoreSlide = BaseSlide + 43;
    public const int SlideList = BaseSlide + 44;
    public static void Configure()
    {
        PermissionsCodes.AddCode("shop", BaseShop);
        PermissionsCodes.AddCode("product", BaseProduct);
        PermissionsCodes.AddCode("productCategory", BaseProductCategory);
        PermissionsCodes.AddCode("productPictures", BaseProductPicture);
        PermissionsCodes.AddCode("slide", BaseSlide);
    }
}