namespace ShepherdsPiesControllers.DTOs;

public class PizzaResponseDto
{
    public int Id { get; set; }
    public required SizeDto Size { get; set; }
    public required CheeseOptionDto CheeseOption { get; set; }
    public required SauceOptionDto SauceOption { get; set; }
    public List<ToppingDto> Toppings { get; set; } = new();
    public decimal Price { get; set; }
}