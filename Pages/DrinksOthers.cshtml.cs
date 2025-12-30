using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Pages;
public class DrinksOthersModel : PageModel
{
    readonly ApplicationDbContext _db;
    public List<RestaurantItem> RestaurantItems { get; set; }
    public DrinksOthersModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task OnGetAsync()
    {
        RestaurantItems = await _db.RestaurantItems.Where(x => x.Ability == AbilityType.Able).Where(y => y.MealType == MealType.Drink
            || y.MealType == MealType.Desert || y.MealType == MealType.Sauce).ToListAsync();
    }
}
