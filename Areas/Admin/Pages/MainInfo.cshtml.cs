using System.Threading.Tasks;
using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Areas.Admin.Pages;

[Authorize(Policy = "AdminPolicy")]
public class MainInfoModel : PageModel
{
    private readonly ApplicationDbContext _db;

    readonly ILogger<MainInfoModel> _logger;
    public MainInfoModel(ApplicationDbContext db, ILogger<MainInfoModel> logger)
    {
        _db = db;
        _logger = logger;
    }

    [BindProperty]
    public Merchant? Merchant { get; set; }

    [BindProperty]
    public OpenHours? OpenHours { get; set; }

    [BindProperty]
    public OpenStatus? OpenStatus { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        OpenStatus = await _db.OpenStatus.FirstOrDefaultAsync();

        OpenHours = await _db.OpenHours.FirstOrDefaultAsync();

        Merchant = await _db.Merchant.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostChangeMerchantDetailsAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _db.Merchant.Add(Merchant);
        await _db.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostChangeHoursAsync()
    {
        _db.OpenStatus.Update(OpenStatus);
        _db.OpenHours.Update(OpenHours);
        await _db.SaveChangesAsync();

        return RedirectToPage();
    }
}  
