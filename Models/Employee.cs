using Microsoft.AspNetCore.Identity;

namespace ShepherdsPiesControllers.Models;

public class Employee : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}