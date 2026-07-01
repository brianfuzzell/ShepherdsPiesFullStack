using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> GetByDateAsync(DateTime date);
    Task<Order?> GetByIdAsync(int id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task CancelAsync(int id);
}