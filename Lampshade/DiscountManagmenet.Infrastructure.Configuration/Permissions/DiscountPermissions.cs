using _0_Framework.Application;
using _0_Framework.Infrastructure;

namespace DiscountManagement.Infrastructure.Configuration.Permissions;

public class DiscountPermissions : IPermissions
{
    public const int DiscountBase = 3000;

    public const int CustomerDiscountBase = DiscountBase + 100;

    public const int DefineCustomerDiscount = CustomerDiscountBase + 01;
    public const int EditCustomerDiscount = CustomerDiscountBase + 02;
    public const int SearchCustomerDiscount = CustomerDiscountBase + 03;
    public const int RemoveAndRestoreCustomerDiscount = CustomerDiscountBase + 04;
    public const int CustomerDiscountList = CustomerDiscountBase + 05;


    public const int ColleagueDiscountBase = DiscountBase + 200;

    public const int DefineColleagueDiscount = ColleagueDiscountBase + 01;
    public const int EditColleagueDiscount = ColleagueDiscountBase + 02;
    public const int SearchColleagueDiscount = ColleagueDiscountBase + 03;
    public const int RemoveAndRestoreColleagueDiscount = ColleagueDiscountBase + 04;
    public const int ColleagueDiscountList = ColleagueDiscountBase + 05;
    public static void Configure()
    {
        PermissionsCodes.AddCode("discount", DiscountBase);
        PermissionsCodes.AddCode("customerDiscount", CustomerDiscountBase);
        PermissionsCodes.AddCode("colleagueDiscount", ColleagueDiscountBase);
    }
}