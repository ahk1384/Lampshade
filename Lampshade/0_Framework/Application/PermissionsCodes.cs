namespace _0_Framework.Application;

public class PermissionsCodes
{
    public static Dictionary<string, int> Codes = new()
    {
        { "Admin", 1 },
        { "shop", 1000 },
        { "product", 1100 },
        { "productCategory", 1200 },
        { "productPictures", 1300 },
        { "slide", 1400 },
        { "inventory", 2000 },
        { "discount", 3000 },
        { "customerDiscount", 3100 },
        { "colleagueDiscount", 3200 },
        { "comment", 4000 },
        { "blog", 5000 },
        { "articles", 5100 },
        { "articlCategories", 5200 },
        { "account", 6000 },
        { "accountManagement", 6100 },
        { "role", 6200 }
    };

    public static int GetCode(string policyName)
    {
        return Codes.TryGetValue(policyName, out var code) ? code : 0;
    }
}