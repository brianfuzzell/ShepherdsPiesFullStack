using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public class SizeRepository : ISizeRepository
{
    private readonly ShepherdsPiesDbContext _context;

    public SizeRepository(ShepherdsPiesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Size>> GetAllAsync()
    {
        return await _context.Sizes.ToListAsync();
    }

    public async Task<Size?> GetByIdAsync(int id)
    {
        return await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
    }
}