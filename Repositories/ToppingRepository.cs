using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public class ToppingRepository : IToppingRepository
{
    private readonly ShepherdsPiesDbContext _context;

    public ToppingRepository(ShepherdsPiesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Topping>> GetAllAsync()
    {
        return await _context.Toppings.ToListAsync();
    }

    public async Task<Topping?> GetByIdAsync(int id)
    {
        return await _context.Toppings.FirstOrDefaultAsync(t => t.Id == id);
    }
}