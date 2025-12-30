using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodTruckApp.Code;
public class StatusOrder
{
    public static List<SelectListItem> StatusList => new()
        {
            new SelectListItem { Value = "1", Text = "Order in process" },
            new SelectListItem { Value = "2", Text = "Prepaired for pick-up" },
            new SelectListItem { Value = "3", Text = "Finished" },
            new SelectListItem { Value = "4", Text = "Storno" }
        };
    public static string Status(int state)
    {
        return state switch
        {
            1 => "Order in process",
            2 => "Prepaired for pick-up",
            3 => "Finished",
            4 => "Storno",
            _ => "unknown"
        };

    }

}