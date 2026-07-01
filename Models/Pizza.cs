namespace ShepherdsPiesControllers.Models;

public class Pizza
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int SizeId { get; set; }
    public Size Size { get; set; } = null!;

    public int CheeseOptionId { get; set; }
    public CheeseOption CheeseOption { get; set; } = null!;

    public int SauceOptionId { get; set; }
    public SauceOption SauceOption { get; set; } = null!;

    public ICollection<PizzaTopping> PizzaToppings { get; set; } = new List<PizzaTopping>();
}