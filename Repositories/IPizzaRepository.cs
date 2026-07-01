using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public interface IPizzaRepository
{
    Task<Pizza?> GetByIdAsync(int id);
    Task AddAsync(Pizza pizza);
    Task UpdateAsync(Pizza pizza);
    Task DeleteAsync(int id);
}