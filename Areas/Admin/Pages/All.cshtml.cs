using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Areas.Admin.Pages;

[Authorize(Policy = "AdminPolicy")]
public class AllModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IEnumerable<NewOrder> NewOrder { get; set; }
    public AllModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task OnGetAsync()
    {
        NewOrder = await _db.NewOrders.Include(o => o.Items).OrderByDescending(o => o.Date).ToListAsync();
    }   
}
