using System.Security.Claims;
using System.Threading.Tasks;
using FoodTruckApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodTruckApp.Pages;

public class LoginModel : PageModel
{
    private readonly ApplicationDbContext _db;

    private readonly ILogger<LoginModel> _logger;

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }
    public bool LoginFailed { get; set; } = false;
    public LoginModel(ApplicationDbContext db, ILogger<LoginModel> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var User = await _db.AdminUsers.FirstOrDefaultAsync(u => u.Username == Username);

        if (User == null || !BCrypt.Net.BCrypt.Verify(Password, User.PasswordHash))
        {
            LoginFailed = true;
            ModelState.AddModelError("", "Invalid login credentials.");
            _logger.LogInformation("Invalid login credentials");
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, User.Username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("role", "Admin")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToPage("/Index");
    }
}
