using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodTruckApp.Code;
public class PayMethod
{
    public static List<SelectListItem> PaymentMethodList => new()
        {
            new SelectListItem { Value = "1", Text = "Cash" },
            new SelectListItem { Value = "2", Text = "Card" },
        };
    public static string Status(int state)
    {
        return state switch
        {
            1 => "Cash",
            2 => "Card",
            _ => "unknown"
        };

    }
}