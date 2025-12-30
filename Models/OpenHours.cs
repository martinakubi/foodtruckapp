namespace FoodTruckApp.Models;

public class OpenHours
{
    public int Id { get; set; }
    public TimeOnly Openhour { get; set; } = new TimeOnly(13, 0, 0);
    public TimeOnly Closehour { get; set; } = new TimeOnly(22, 0, 0);
}