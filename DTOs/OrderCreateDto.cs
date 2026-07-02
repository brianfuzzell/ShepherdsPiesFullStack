using System.ComponentModel.DataAnnotations;

namespace ShepherdsPiesControllers.DTOs;

public class OrderCreateDto
{
    [Required]
    public required string OrderType { get; set; }   // "DineIn" or "Delivery"

    public int? TableNumber { get; set; }             // required for DineIn, checked in the controller

    [Required]
    public required string EmployeeId { get; set; }   // order taker
}