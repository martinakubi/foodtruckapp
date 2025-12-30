using System.Text.Json;

namespace FoodTruckApp.Code;
public static class OrderCookieHelper
{
    private const string CookieName = "activeOrders";

    public static void AddOrderToCookie(HttpContext context, int orderId)
    {
        List<int> orders = new();

        if (context.Request.Cookies.TryGetValue(CookieName, out var existing))
        {
            try
            {
                orders = JsonSerializer.Deserialize<List<int>>(existing) ?? new List<int>();
            }
            catch
            {
                orders = new List<int>();
            }
        }

        // Přidej nové ID jen pokud tam už není
        if (!orders.Contains(orderId))
            orders.Add(orderId);

        // Ulož zpět jako JSON
        var options = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddHours(2),
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict
        };

        var serialized = JsonSerializer.Serialize(orders);
        context.Response.Cookies.Append(CookieName, serialized, options);
    }

    public static List<int> GetActiveOrdersFromCookie(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue(CookieName, out var existing))
        {
            try
            {
                return JsonSerializer.Deserialize<List<int>>(existing) ?? new List<int>();
            }
            catch
            {
                return new List<int>();
            }
        }

        return new List<int>();
    }
}

