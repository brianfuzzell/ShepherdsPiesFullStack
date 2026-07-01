using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public class CheeseOptionRepository : ICheeseOptionRepository
{
    private readonly ShepherdsPiesDbContext _context;

    public CheeseOptionRepository(ShepherdsPiesDbContext context)
    {
        _context = context;
    }

    public async Task<List<CheeseOption>> GetAllAsync()
    {
        return await _context.CheeseOptions.ToListAsync();
    }

    public async Task<CheeseOption?> GetByIdAsync(int id)
    {
        return await _context.CheeseOptions.FirstOrDefaultAsync(c => c.Id == id);
    }
}