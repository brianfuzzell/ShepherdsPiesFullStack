using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public class SauceOptionRepository : ISauceOptionRepository
{
    private readonly ShepherdsPiesDbContext _context;

    public SauceOptionRepository(ShepherdsPiesDbContext context)
    {
        _context = context;
    }

    public async Task<List<SauceOption>> GetAllAsync()
    {
        return await _context.SauceOptions.ToListAsync();
    }

    public async Task<SauceOption?> GetByIdAsync(int id)
    {
        return await _context.SauceOptions.FirstOrDefaultAsync(s => s.Id == id);
    }
}