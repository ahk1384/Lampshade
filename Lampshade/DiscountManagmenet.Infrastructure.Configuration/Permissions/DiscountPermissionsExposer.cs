using _0_Framework.Infrastructure;

namespace DiscountManagement.Infrastructure.Configuration.Permissions;

public class DiscountPermissionsExposer : IPermissionExposer
{
    public Dictionary<string, List<PermissionDto>> Expose()
    {
        return new Dictionary<string, List<PermissionDto>>
        {
            {
                "Customer Discount", new List<PermissionDto>
                {
                    new (DiscountPermissions.CustomerDiscountList,"CustomerDiscount List"),
                    new(DiscountPermissions.DefineCustomerDiscount, "DefineCustomerDiscount"),
                    new(DiscountPermissions.EditCustomerDiscount, "EditCustomerDiscount"),
                    new(DiscountPermissions.SearchCustomerDiscount, "SearchCustomerDiscount"),
                    new(DiscountPermissions.RemoveAndRestoreCustomerDiscount, "RemoveAndRestoreCustomerDiscount")
                }
            },
            {
                "Colleague Discount", new List<PermissionDto>
                {
                    new (DiscountPermissions.ColleagueDiscountList,"ColleagueDiscount List"),
                    new(DiscountPermissions.DefineColleagueDiscount, "DefineColleagueDiscount"),
                    new(DiscountPermissions.EditColleagueDiscount, "EditColleagueDiscount"),
                    new(DiscountPermissions.SearchColleagueDiscount, "SearchColleagueDiscount"),
                    new(DiscountPermissions.RemoveAndRestoreColleagueDiscount, "RemoveAndRestoreColleagueDiscount")
                }
            },
        };
    }
}