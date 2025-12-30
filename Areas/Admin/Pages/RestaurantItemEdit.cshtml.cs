using System.ComponentModel.DataAnnotations.Schema;
using FoodTruckApp.Data;
using FoodTruckApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodTruckApp.Code;

namespace FoodTruckApp.Areas.Admin.Pages;

[Authorize(Policy = "AdminPolicy")]
public class RestaurantItemEditModel : PageModel
{
    private readonly IWebHostEnvironment _environment;
    private readonly ApplicationDbContext DB;

    public RestaurantItemEditModel(ApplicationDbContext db, IWebHostEnvironment environment)
    {
        DB = db;
        _environment = environment;
    }

    [BindProperty]
    public RestaurantItem ResItem { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int ItemId { get; set; }

    [BindProperty]
    public IFormFile? ImageUpload { get; set; }

    public IEnumerable<SelectListItem> MealTypeOptions { get; set; } = Enumerable.Empty<SelectListItem>();

    public async Task OnGetAsync()
    {
        if (ItemId == 0)
        {
            ResItem = new RestaurantItem();
        }
        else
        {
            ResItem = await DB.RestaurantItems.FirstOrDefaultAsync(r => r.Id == ItemId);
        }

        MealTypeOptions = Enum.GetValues(typeof(MealType))
            .Cast<MealType>()
            .Select(m => new SelectListItem
            {
                Value = ((int)m).ToString(),
                Text = m.ToString()
            });
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ResItem.Kod = MyUrl.Status(ResItem.Name);

        ModelState.ClearValidationState(nameof(ResItem));

        if (ResItem == null || !TryValidateModel(ResItem, nameof(ResItem)))
        {
            await OnGetAsync();
            return Page();
        }

        if (ImageUpload != null && ImageUpload.Length > 0)
        {
            var fileName = $"{ResItem.Id}{Path.GetExtension(ImageUpload.FileName)}";
            var filePath = Path.Combine(_environment.WebRootPath, "pictures", fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await ImageUpload.CopyToAsync(stream);

            ResItem.ImagePath = $"/pictures/{fileName}";
        }

        RestaurantItem dbItem;

        if (ItemId == 0)
        {
            ResItem.Kod = MyUrl.Status(ResItem.Name);
            dbItem = ResItem;
            await DB.AddAsync(dbItem);
        }
        else
        {
            dbItem = await DB.RestaurantItems
                .FirstOrDefaultAsync(r => r.Id == ItemId);

            if (dbItem == null)
                return NotFound();

            // Update values
            dbItem.Name = ResItem.Name;
            dbItem.Kod = MyUrl.Status(ResItem.Name);
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                // tady si můžeš uložit cestu do db
                dbItem.ImagePath = ResItem.ImagePath;
            }
            dbItem.Price = ResItem.Price;
            dbItem.Description = ResItem.Description;
            dbItem.MealType = ResItem.MealType;
            dbItem.Ability = ResItem.Ability;

            DB.Update(dbItem);
        }

        await DB.SaveChangesAsync();
        return Redirect($"/menu/{dbItem.Kod}");
    }

    public async Task<IActionResult> OnPostDeleteAsync(string ItemIdDel)
    {
        if (int.TryParse(ItemIdDel, out int itemId) && itemId > 0)
        {
            var item = await DB.RestaurantItems.FindAsync(itemId);
            if (item != null)
            {
                DB.RestaurantItems.Remove(item);
                await DB.SaveChangesAsync();
            }
        }
        return Redirect("/");
    }

}
