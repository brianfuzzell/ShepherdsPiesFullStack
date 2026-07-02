namespace ShepherdsPiesControllers.Models;

public class PizzaTopping
{
    public int Id { get; set; }

    public int PizzaId { get; set; }
    public Pizza Pizza { get; set; } = null!;

    public int ToppingId { get; set; }
    public Topping Topping { get; set; } = null!;
}