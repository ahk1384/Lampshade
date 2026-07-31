using AccountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using ShopManagement.Application.Contracts.Report;
using SM.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class ReportQuery : IReportQuery
{
    private AccountContext _accountContext;
    private InventoryContext _inventoryContext;
    private ShopContext _shopContext;

    public ReportQuery(ShopContext shopContext, InventoryContext inventoryContext, AccountContext accountContext)
    {
        _shopContext = shopContext;
        _inventoryContext = inventoryContext;
        _accountContext = accountContext;
    }

    public double GetTotalSell()
    {
        var sells = _shopContext.Orders.Where(x =>
                x.IsPaid && !x.IsCanceled &&
                (x.CreationDate > DateTime.Now.AddDays(-15) && x.CreationDate < DateTime.Now))
            .Sum(x => x.PayAmount);
        return sells;
    }

    public double GetTotalBuy()
    {
        var possibleToSell = _inventoryContext.Inventories
            .Select(x => new { x.UnitPrice, CalculateCurrentCount = x.CalculateCurrentCount() }).ToList();
        var possible = possibleToSell.Sum(x => x.CalculateCurrentCount * x.UnitPrice);
        var selled = _shopContext.Orders.Where(x =>
            x.IsPaid && !x.IsCanceled).Sum(x => x.PayAmount);
        return possible + selled;
    }

    public int GetPrecentSell()
    {
        var possible = GetTotalBuy();
        var selled = _shopContext.Orders.Where(x =>
            x.IsPaid && !x.IsCanceled).Sum(x => x.PayAmount);
        return (int)Math.Round((selled) / possible * 100);
    }

    public int NewOrders()
    {
        var orders = _shopContext.Orders
            .Where(x => x.IsPaid && x.IsDeleted == false && x.CreationDate > DateTime.Now.AddDays(-15)).Count();
        return orders;
    }

    public int NewUsers()
    {
        var users = _accountContext.Accounts.Where(x => !x.IsDeleted && x.CreationDate > DateTime.Now.AddDays(-15))
            .Count();
        return users;
    }

    public List<double> SellPerMounths()
    {
        double[] list = new Double[14];
        var items = _shopContext.Orders.Where(x => x.CreationDate <= DateTime.Now &&
                                                   x.CreationDate >= DateTime.Now.AddYears(-1)).ToList();
        for (int i = 0; i < 12; i++)
        {
            int x = (i + 10) % 12;
            list[x] = items.Where(x =>
                x.CreationDate.Month > i && x.CreationDate.Month <= i + 1).Count();
        }

        return list.ToList();
    }
}