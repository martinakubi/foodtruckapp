using FoodTruckApp.Code;
using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodTruckApp.Areas.Admin.Pages;

public class CashSysModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly ApplicationDbContext _db;

    private readonly ILogger<CashSysModel> _logger;
    public string Message { get; private set; }

    [BindProperty]
    public int? PaymentMethod {get;set;}
    public List<RestaurantItem> RestaurantItems { get; set; } = new();
    public List<OrderNewItem> NewOrderItems { get; set; }
    public NewOrder Order { get; set; }
    public RestaurantItem RestaurantItem { get; set; }
    public string GeneratedUserName { get; set; }

    [BindProperty]
    public string UserName { get; set; }

    [BindProperty]
    public string? kennitala { get; set; }

    public Kennitala Kennitalas { get; set; }

    [BindProperty]
    public string? PaymentType { get; set; }

    public Merchant Merchant { get; set; }
    public CashSysModel(ApplicationDbContext db, ILogger<CashSysModel> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        RestaurantItems = await _db.RestaurantItems.OrderBy(x => x.Name).ToListAsync();

        NewOrderItems = OrderNew.UploadNewOrder(Request, _db);

        var lastOrder = await _db.NewOrders.OrderByDescending(o => o.Id).Where(o => o.UserName.StartsWith("cashsys")).FirstOrDefaultAsync();

        int nextNumber = 1;

        if (lastOrder != null)
        {
            var lastNumber = int.Parse(lastOrder.UserName.Replace("cashsys", ""));
            nextNumber = lastNumber % 15 + 1;
        }

        GeneratedUserName = $"cashsys{nextNumber}";
        _logger.LogInformation($"Order user name: {GeneratedUserName}");
        
        return Page();
    }
    public IActionResult OnPost(string itemId, string operation, string price, string guild)
    {
        if (!int.TryParse(itemId, out var itId))
        {
            return RedirectToPage();
        }    

        var guiId = Guid.TryParse(guild, out var parsed) ? parsed : Guid.Empty;
        
        if (operation == "x")
        {
            if (!decimal.TryParse(price, System.Globalization.NumberStyles.Any, 
                     System.Globalization.CultureInfo.InvariantCulture, out var pr)) 
                pr = 0;
            _logger.LogInformation("Operation delete was successfull.");

            NewOrderItems = OrderNew.RemoveItem(Request, Response, guiId, _db);
        }

        else if (operation == "+" || operation == "-")
        {
            NewOrderItems = OrderNew.AddItemNewOrder(Request, Response, new OrderNewItem()
            {
                Id = Guid.NewGuid(),
                RestaurantItemId = itId,
                NumOfPortions = (operation == "+" ? 1 : -1)
            }, _db);
            _logger.LogInformation("Operation + - was successfull.");
        }

        else if (operation == "c")
        {
            if (!decimal.TryParse(Request.Form["newPrice"], out var newPrice))
                newPrice = 0;

            NewOrderItems = OrderNew.ChangeItemPrice(
                Request,
                Response,
                new OrderNewItem { RestaurantItemId = itId, NumOfPortions = 1 },
                _db,
                newPrice
            );

            _logger.LogInformation("Operation change price was successfull.");
        }

        else
            NewOrderItems = OrderNew.UploadNewOrder(Request, _db);

        return RedirectToPage();
    }
    public IActionResult OnPostOrder(string operation)
    {
        if (operation == "delete")
        {
            NewOrderItems = OrderNew.DeleteNewOrder(Response);
            return RedirectToPage();
        }
        return RedirectToPage();
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostOrderJsonAsync()
    {
        var paymentmethod = int.TryParse(PaymentType, out var paymet) ? paymet : 0;

        NewOrderItems = OrderNew.UploadNewOrder(Request, _db);

        Merchant = await _db.Merchant.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

        if(Merchant == null)
        {
            _logger.LogInformation("Merchant data are not created.");
            return Redirect("/admin/maininfo");
        }
        
        if (NewOrderItems.Count == 0 || !ModelState.IsValid) 
        {
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var err in errors)
                {
                    _logger.LogError($"ModelState error: {key} - {err.ErrorMessage}");
                }
            }

            return new JsonResult(new { success = false, error = "Invalid model or empty order." });
        }

        var ord = new NewOrder()
        {
            Date = DateTime.UtcNow,
            State = 1,
            UserName = this.UserName,
            PaymentMethod = paymentmethod,
            MerchantId = Merchant.Id,
        };

        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            await _db.NewOrders.AddAsync(ord);
            await _db.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(kennitala))
                {
                    var kenni = new Kennitala()

                    {
                        KennitalaNumber = kennitala,
                        NewOrderId = ord.Id,
                    };
                    await _db.Kennitalas.AddAsync(kenni);
                    await _db.SaveChangesAsync();
            }

            var itemord = NewOrderItems.Select(k => new ItemOrder()
            {
                NewOrderId = ord.Id,
                RestaurantItemId = k.RestaurantItemId,
                PricePerOne = k.PricePerOne,
                NumOfDishes = k.NumOfPortions,
            }).ToList();

            await _db.ItemsOfOrders.AddRangeAsync(itemord);
            await _db.SaveChangesAsync();

            await transaction.CommitAsync();

            //OrderCookieHelper.AddOrderToCookie(HttpContext, ord.Id);
            OrderNew.DeleteNewOrder(Response);

            return new JsonResult(new { success = true, orderId = ord.Id, orderDate = ord.Date, userName = ord.UserName, ordkenni = kennitala, merchantname = Merchant.Name, merchantadress = Merchant.Address, 
            merchantphone = Merchant.PhoneNumber, merchantemail = Merchant.EmailAddress, merchantkennitala = Merchant.Kennitala, merchantvsk = Merchant.VSKNumber});
        }
        
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            var errorMessage = ex.Message;
            if (ex.InnerException != null)
                errorMessage += " | Inner: " + ex.InnerException.Message;
            return new JsonResult(new { success = false, error = errorMessage });
        }
    }
}