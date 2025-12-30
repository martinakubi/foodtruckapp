using FoodTruckApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Areas.Admin.Pages;

[Authorize(Policy = "AdminPolicy")]
public class ChartsModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IEnumerable<MonthlyRevenue> MonthlyRevenuesView { get; set; } = new List<MonthlyRevenue>();
    public List<DailyRevenue> DailyRevenuesView { get; set; } = new();
    public List<string> Labels { get; set; } = new();
    public List<int> Values { get; set; } = new();
    public ChartsModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task OnGetAsync()
    {
        var orders = await _db.NewOrders.Where(o => o.State != 4).Include(o => o.Items).ToListAsync();

        MonthlyRevenuesView = orders
        .AsEnumerable()
        .GroupBy(o => new { o.Date.Year, o.Date.Month })
        .Select(g => new MonthlyRevenue
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            TotalRevenue = g.Sum(order => order.Items.Sum(i => i.NumOfDishes * i.PricePerOne)),
            TotalRevenueCard = g.Where(order => order.PaymentMethod == 2).Sum(order => order.Items.Sum(i => i.NumOfDishes * i.PricePerOne)),
            TotalRevenueNoVat = g.Select(order => (int)Math.Round(order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) / 1.11m, MidpointRounding.AwayFromZero)).Sum(),
            TotalVAT = g.Sum(order => order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) - (int)Math.Round(order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) / 1.11m, MidpointRounding.AwayFromZero)),
            DailyRevenues = g.GroupBy(o => o.Date.Day)
                                 .Select(d => new DailyRevenue
                                 {
                                     Year = g.Key.Year,
                                     Month = g.Key.Month,
                                     Day = d.Key,
                                     TotalRevenue = d.Sum(order => order.Items.Sum(i => i.NumOfDishes * i.PricePerOne)),
                                     TotalRevenueCard = d.Where(order => order.PaymentMethod == 2).Sum(order => order.Items.Sum(i => i.NumOfDishes * i.PricePerOne)),
                                     TotalRevenueNoVat = d.Select(order => (int)Math.Round(order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) / 1.11m, MidpointRounding.AwayFromZero)).Sum(),
                                     TotalVAT = d.Sum(order => order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) - (int)Math.Round(order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) / 1.11m, MidpointRounding.AwayFromZero)),
                                 })
                                 .OrderBy(d => d.Day)
                                 .ToList()
        })
            .OrderByDescending(m => m.Year)
            .ThenByDescending(m => m.Month)
            .ToList();

        Labels = MonthlyRevenuesView.OrderBy(o => o.Year).ThenBy(o => o.Month).Select(o => o.MonthName).ToList();

        Values = MonthlyRevenuesView.OrderBy(o => o.Year).ThenBy(o => o.Month).Select(o => (int)o.TotalRevenue).ToList();
    }
    public async Task OnPostAsync()
    {
        if (Request.Form.TryGetValue("month", out var monthValue))
        {
            int selectedMonth = int.Parse(monthValue);

            var orders = await _db.NewOrders
            .Where(o => o.State != 4 && o.Date.Month == selectedMonth)
            .Include(o => o.Items)
            .ToListAsync();

            DailyRevenuesView = orders
            .AsEnumerable()
            .GroupBy(o => o.Date.Day)
            .Select(g => new DailyRevenue
            {
                Day = g.Key,
                TotalRevenue = g.Sum(o => o.Items.Sum(i => i.NumOfDishes * i.PricePerOne)),
                TotalRevenueCard = g.Where(order => order.PaymentMethod == 2).Sum(order => order.Items.Sum(i => i.NumOfDishes * i.PricePerOne)),
                TotalRevenueNoVat = g.Select(order => (int)Math.Round(order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) / 1.11m, MidpointRounding.AwayFromZero)).Sum(),
                TotalVAT = g.Sum(order => order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) - (int)Math.Round(order.Items.Sum(i => i.NumOfDishes * i.PricePerOne) / 1.11m, MidpointRounding.AwayFromZero)),
            })
            .OrderBy(d => d.Day)
            .ToList();

        Labels = DailyRevenuesView.OrderBy(o => o.Year).ThenBy(o => o.Month).Select(o => o.Day.ToString()).ToList();

        Values = DailyRevenuesView.OrderBy(o => o.Year).ThenBy(o => o.Month).Select(o => (int)o.TotalRevenue).ToList();
        }
    }
    public class MonthlyRevenue
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueCard { get; set; }
        public decimal TotalRevenueNoVat { get; set; }
        public decimal TotalVAT { get; set; }
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM yyyy");
        public List<DailyRevenue> DailyRevenues { get; set; } = new();
    }
    public class DailyRevenue
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueNoVat { get; set; }
        public decimal TotalRevenueCard { get; set; }
        public decimal TotalVAT { get; set; }
    }
}
