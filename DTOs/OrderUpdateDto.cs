using System.ComponentModel.DataAnnotations;

namespace ShepherdsPiesControllers.DTOs;

public class OrderUpdateDto
{
    [Required]
    public required string DeliveryEmployeeId { get; set; }
}