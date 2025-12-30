using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Pages;
public class MainModel : PageModel
{
    readonly ApplicationDbContext _db;
    public List<RestaurantItem> RestaurantItems { get; set; }
    public MainModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task OnGetAsync()
    {
        RestaurantItems = await _db.RestaurantItems.Where(x => x.Ability == AbilityType.Able).Where(y => y.MealType == MealType.Main || y.MealType == MealType.Soup).ToListAsync();
    }
}
