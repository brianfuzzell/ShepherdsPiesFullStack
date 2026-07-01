namespace ShepherdsPiesControllers.Models;

public class Topping
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}