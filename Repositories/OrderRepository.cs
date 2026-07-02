using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ShepherdsPiesDbContext _context;

    public OrderRepository(ShepherdsPiesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetByDateAsync(DateTime date)
    {
        return await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.DeliveryEmployee)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.Size)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.CheeseOption)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.SauceOption)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.PizzaToppings)
                    .ThenInclude(pt => pt.Topping)
            .Where(o => o.OrderDate.Date == date.Date && !o.IsCancelled)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Employee)
            .Include(o => o.DeliveryEmployee)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.Size)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.CheeseOption)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.SauceOption)
            .Include(o => o.Pizzas)
                .ThenInclude(p => p.PizzaToppings)
                    .ThenInclude(pt => pt.Topping)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task CancelAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order is null)
        {
            return;
        }

        order.IsCancelled = true;
        await _context.SaveChangesAsync();
    }
}