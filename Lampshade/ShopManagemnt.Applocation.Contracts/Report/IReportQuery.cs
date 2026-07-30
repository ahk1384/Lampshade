namespace ShopManagement.Application.Contracts.Report;

public interface IReportQuery
{
    double GetTotalSell();
    double GetTotalBuy();
    int GetPrecentSell();
    int NewOrders();
    int NewUsers();
    List<double> SellPerMounths();
}