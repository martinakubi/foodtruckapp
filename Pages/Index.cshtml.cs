using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly ApplicationDbContext _db;
    public List<RestaurantItem> RestaurantItems { get; set; }
    public bool IsOpen { get; set; }
    public TimeOnly? Openhours { get; set; }
    public TimeOnly? Closehours { get; set; }
    public Merchant? Merchant { get; set; }
    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }
    public async Task OnGetAsync()
    {
        RestaurantItems = await _db.RestaurantItems
        .Where(x => x.Ability == AbilityType.Able)
        .Where(z => !z.ImagePath.StartsWith("~/pictures"))
        .Where(y => y.MealType == MealType.Main || y.MealType == MealType.Soup)
        .ToListAsync();

        IsOpen = await _db.OpenStatus
        .OrderByDescending(x => x.Id)
        .Select(x => x.IsOpen)
        .FirstOrDefaultAsync();

        Openhours = await _db.OpenHours
        .OrderByDescending(x => x.Id)
        .Select(x => x.Openhour)
        .FirstOrDefaultAsync();

        Closehours = await _db.OpenHours
        .OrderByDescending(x => x.Id)
        .Select(x => x.Closehour)
        .FirstOrDefaultAsync();

        Merchant = await _db.Merchant.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
    }
}
