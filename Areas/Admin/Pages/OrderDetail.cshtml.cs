using System.Data;
using System.Text.Json.Serialization;
using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Areas.Admin.Pages;

[Authorize(Policy = "AdminPolicy")]
public class OrderDetailModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;

    [BindProperty]
    public NewOrder NewOrder { get; set; }

    [BindProperty(SupportsGet = true)]
    public int OrderId { get; set; }
    public RestaurantItem? restaurantItem { get; set; }
    public Kennitala? Kennitala { get; set; }
    public Merchant? Merchant { get; set; }
    public OrderDetailModel(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }
    public async Task OnGetAsync()
    {
        NewOrder = await _db.NewOrders.Include(o => o.Items).ThenInclude(p => p.RestaurantItem).FirstOrDefaultAsync(x => x.Id == OrderId);

        Kennitala = await _db.Kennitalas.FirstOrDefaultAsync(y => y.NewOrderId == OrderId);

        Merchant = await _db.Merchant.Where(x => x.Id == NewOrder.MerchantId).FirstOrDefaultAsync();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var order = await _db.NewOrders.FirstOrDefaultAsync(x => x.Id == OrderId);

        if (order == null)
            return NotFound();
            
        order.PaymentMethod = NewOrder.PaymentMethod;
        order.State = NewOrder.State;
        order.StornoNote = NewOrder.StornoNote;

        _db.NewOrders.Update(order);
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }
    public async Task<IActionResult> OnPostPrintAsync()
    {
        var order = await _db.NewOrders.Include(o => o.Items).ThenInclude(p => p.RestaurantItem).FirstOrDefaultAsync(x => x.Id == OrderId);

        if (order == null)
            return NotFound();

        var kenni = await _db.Kennitalas.FirstOrDefaultAsync(y => y.NewOrderId == OrderId);

        return new JsonResult(new
        {
            orderId = order.Id,
            orderDate = order.Date,
            ordkenni = kenni?.KennitalaNumber,
            userName = order.UserName,
        });
    }
}
