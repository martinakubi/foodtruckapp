using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Pages;
public class AlergenesModel : PageModel
{
    readonly ApplicationDbContext _db;
    public List<AlergenesTypes> AlergenesTypeList { get; set; }
    public AlergenesModel(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task OnGetAsync()
    {
        AlergenesTypeList = await _db.AlergenesTypes.ToListAsync();
    }
}
