using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public interface IToppingRepository
{
    Task<List<Topping>> GetAllAsync();
    Task<Topping?> GetByIdAsync(int id);
}