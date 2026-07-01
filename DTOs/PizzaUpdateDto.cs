using System.ComponentModel.DataAnnotations;

namespace ShepherdsPiesControllers.DTOs;

public class PizzaUpdateDto
{
    [Required]
    public int SizeId { get; set; }

    [Required]
    public int CheeseOptionId { get; set; }

    [Required]
    public int SauceOptionId { get; set; }

    public List<int> ToppingIds { get; set; } = new();
}