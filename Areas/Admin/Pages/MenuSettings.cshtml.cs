using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FoodTruckApp.Areas.Admin.Pages;

[Authorize(Policy = "AdminPolicy")]
public class MenuSettingsModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public MenuSettingsModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public List<RestaurantItem> RestaurantItems { get; set; }
    public async Task OnGetAsync()
    {
        RestaurantItems = await _db.RestaurantItems.OrderBy(x => x.Id).ToListAsync();
    }
}
