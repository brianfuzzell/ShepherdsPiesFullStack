namespace ShepherdsPiesControllers.DTOs;

public class OrderResponseDto
{
    public int Id { get; set; }
    public required string OrderType { get; set; }
    public int? TableNumber { get; set; }

    public required string EmployeeId { get; set; }
    public required string EmployeeName { get; set; }

    public string? DeliveryEmployeeId { get; set; }
    public string? DeliveryEmployeeName { get; set; }

    public DateTime OrderDate { get; set; }
    public bool IsCancelled { get; set; }

    public List<PizzaResponseDto> Pizzas { get; set; } = new();
    public decimal Total { get; set; }
}