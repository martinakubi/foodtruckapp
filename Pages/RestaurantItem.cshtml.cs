using System.Threading.Tasks;
using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Pages;
public class RestaurantItemModel : PageModel
{
    readonly ApplicationDbContext _db;

    readonly ILogger<RestaurantItemModel> _logger;
    public RestaurantItem RestaurantItem { get; set; }

    [BindProperty(SupportsGet = true)]
    public string Kod { get; set; }
    public RestaurantItemModel(ApplicationDbContext db, ILogger<RestaurantItemModel> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task OnGetAsync()
    {
        RestaurantItem = await _db.RestaurantItems.FirstOrDefaultAsync(x => x.Kod == Kod);
    }
}
