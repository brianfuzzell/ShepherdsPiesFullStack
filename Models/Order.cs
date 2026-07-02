namespace ShepherdsPiesControllers.Models;

public class Order
{
    public int Id { get; set; }
    public required string OrderType { get; set; }   // "DineIn" or "Delivery"
    public int? TableNumber { get; set; }             // nullable, DineIn only

    public required string EmployeeId { get; set; }   // order taker, required
    public required Employee Employee { get; set; }

    public string? DeliveryEmployeeId { get; set; }   // nullable, Delivery only
    public Employee? DeliveryEmployee { get; set; }

    public DateTime OrderDate { get; set; }
    public bool IsCancelled { get; set; }

    public ICollection<Pizza> Pizzas { get; set; } = new List<Pizza>();
}