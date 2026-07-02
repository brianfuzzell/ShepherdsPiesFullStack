using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public class PizzaRepository : IPizzaRepository
{
    private readonly ShepherdsPiesDbContext _context;

    public PizzaRepository(ShepherdsPiesDbContext context)
    {
        _context = context;
    }

    public async Task<Pizza?> GetByIdAsync(int id)
    {
        return await _context.Pizzas
            .Include(p => p.Size)
            .Include(p => p.CheeseOption)
            .Include(p => p.SauceOption)
            .Include(p => p.PizzaToppings)
                .ThenInclude(pt => pt.Topping)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pizza pizza)
    {
        _context.Pizzas.Update(pizza);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pizza = await _context.Pizzas.FindAsync(id);
        if (pizza is null)
        {
            return;
        }

        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();
    }
}