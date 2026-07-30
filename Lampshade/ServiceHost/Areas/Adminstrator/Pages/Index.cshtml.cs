using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopManagement.Application.Contracts.Report;

namespace ServiceHost.Areas.Adminstrator.Pages;

public class IndexModel : PageModel
{
    private IReportQuery _reportQuery;

    public IndexModel(IReportQuery reportQuery)
    {
        _reportQuery = reportQuery;
    }

    public Chart DoughnutDataSet { get; set; }
    public Chart BarLineDataSet { get; set; }

    public double totalSale { get; set; }
    public int newOrders { get; set; }
    public int newUsers { get; set; }
    public double precentsell { get; set; }

    public double totalBuy { get; set; }

    public void OnGet()
    {
        totalSale = _reportQuery.GetTotalSell();
        precentsell = _reportQuery.GetPrecentSell();
        newOrders = _reportQuery.NewOrders();
        newUsers = _reportQuery.NewUsers();
        totalBuy = _reportQuery.GetTotalBuy();
        BarLineDataSet = new Chart();
        DoughnutDataSet = new Chart();
        BarLineDataSet.labels.AddRange([
            "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", " دی", "بهمن", "اسفند"
        ]);
        BarLineDataSet.Items.Add(new chartItem()
        {
            Label = "فروش",
            Data = _reportQuery.SellPerMounths(),
            BackgroundColor = new[] { "##D7ECFB" },
            BorderColor = new[] { "#2196f3" },
            fill = false,
            tension = 0.4
        });
        // BarLineDataSet.Items.Add(new chartItem()
        // {
        //     Label = "Samsung",
        //     Data = new List<double> { 200, 300, 350, 270, 100 },
        //     BackgroundColor = new[] { "#ffc8dd" },
        //     BorderColor = new[] { "#ffafcc" },
        //     fill = false,
        //     tension = 0.4
        // });
        // BarLineDataSet.Items.Add(new chartItem()
        // {
        //     Label = "Total",
        //     Data = new List<double> { 300, 500, 600, 440, 150 },
        //     BackgroundColor = new[] { "#0077b6" },
        //     BorderColor = new[]{"#023e8a"},
        //     fill = false,
        //     tension = 0.4
        // });
        DoughnutDataSet.labels.Add("Apple");
        DoughnutDataSet.Items.Add(new chartItem
        {
            Label = "Apple",
            Data = new List<double> { 100, 200, 250, 170, 50 },
            BorderColor = new[] { "#ffcdb2" },
            BackgroundColor = new[] { "#b5838d", "#ffd166", "#7f4f24", "#ef233c", "#003049" }
        });
    }
}