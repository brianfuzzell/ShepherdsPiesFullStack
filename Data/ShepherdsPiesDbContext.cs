using Microsoft.EntityFrameworkCore;

namespace ShepherdsPiesControllers.Data;

public class ShepherdsPiesDbContext : DbContext
{
    public ShepherdsPiesDbContext(DbContextOptions<ShepherdsPiesDbContext> options) : base(options)
    {
    }
}