using FoodTruckApp.Data;
using Newtonsoft.Json;

namespace FoodTruckApp.Code;

public record OrderNewItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int RestaurantItemId { get; set; }

    public int NumOfPortions { get; set; } = 1;

    [JsonIgnore]
    public string Name { get; set; }

    public decimal PricePerOne { get; set; }

}

public static class OrderNew
{
    public static List<OrderNewItem> UploadNewOrder (HttpRequest request, ApplicationDbContext db)
    {
        if(!request.Cookies.TryGetValue("neworder", out string order) || String.IsNullOrEmpty(order)) 
            return new List<OrderNewItem> ();
        var newOrder = JsonConvert.DeserializeObject<List<OrderNewItem>>(order);
        UploadItemDetails(newOrder, db);
        Console.WriteLine(JsonConvert.SerializeObject(newOrder, Formatting.Indented));
        return newOrder;
    }

    static void UploadItemDetails(List<OrderNewItem> newOrder, ApplicationDbContext db)
    {
        if (db == null) return;

        var idItemInOrder = newOrder.Select(k => k.RestaurantItemId);

        var item = db.RestaurantItems.Where(z => idItemInOrder.Contains(z.Id)).ToList();
        
        newOrder.ForEach(k =>
        {
            var z = item.FirstOrDefault(z => z.Id == k.RestaurantItemId);
            k.Name = z?.Name;
            if (k.PricePerOne == 0)
                k.PricePerOne = z?.Price ?? 0;
        });
        
    }

    public static void SaveNewOrder (HttpResponse response, List<OrderNewItem> newOrder)
    {
        newOrder.RemoveAll(x => x.NumOfPortions <= 0);

        string json = JsonConvert.SerializeObject(newOrder);

        response.Cookies.Append("neworder", json);
    }

    public static List<OrderNewItem> RemoveItem (HttpRequest request, HttpResponse response, Guid guidId, ApplicationDbContext db)
    {
        var newOrder = UploadNewOrder(request, db);

        Console.WriteLine("Before remove: " + JsonConvert.SerializeObject(newOrder, Formatting.Indented));
        newOrder.RemoveAll(k => k.Id == guidId);

        Console.WriteLine("After remove: " + JsonConvert.SerializeObject(newOrder, Formatting.Indented));
        SaveNewOrder(response, newOrder);
        return newOrder;
    }


    public static List<OrderNewItem> AddItemNewOrder (HttpRequest request, HttpResponse response, OrderNewItem newItem, ApplicationDbContext db = null)
    {
        var newOrder = UploadNewOrder(request, db);

        var itemOrder = newOrder.FirstOrDefault(k => k.RestaurantItemId == newItem.RestaurantItemId);
        
        if (itemOrder == null)
        {
            UploadItemDetails(new List<OrderNewItem> { newItem }, db);
            newOrder.Add(newItem);
        }
        else
            itemOrder.NumOfPortions += newItem.NumOfPortions;
        SaveNewOrder(response, newOrder);
        return newOrder;
    }
    
    public static List<OrderNewItem> ChangeItemPrice (HttpRequest request, HttpResponse response, OrderNewItem newItem, ApplicationDbContext db = null, decimal newPrice = 0)
    {
        var newOrder = UploadNewOrder(request, db);

        var itemOrder = newOrder.FirstOrDefault(k => k.RestaurantItemId == newItem.RestaurantItemId);

        if (itemOrder != null && itemOrder.NumOfPortions > 0 && itemOrder.PricePerOne != newPrice)
        {
            itemOrder.NumOfPortions -= 1;

            var newItemRecord = new OrderNewItem
            {
                RestaurantItemId = itemOrder.RestaurantItemId,
                NumOfPortions = 1,
                PricePerOne = newPrice,
                Name = itemOrder.Name
            };

            newOrder.Add(newItemRecord);
            SaveNewOrder(response, newOrder);
        }

            return newOrder;
    }

    public static List<OrderNewItem> DeleteNewOrder(HttpResponse response)
    {
        response.Cookies.Delete("neworder");
        return new List<OrderNewItem>();
    }
}