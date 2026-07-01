using System.ComponentModel.DataAnnotations;

namespace ShepherdsPiesControllers.DTOs;

public class PizzaCreateDto
{
    [Required]
    public int SizeId { get; set; }

    [Required]
    public int CheeseOptionId { get; set; }

    [Required]
    public int SauceOptionId { get; set; }

    public List<int> ToppingIds { get; set; } = new();
}