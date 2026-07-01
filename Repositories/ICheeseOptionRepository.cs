using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public interface ICheeseOptionRepository
{
    Task<List<CheeseOption>> GetAllAsync();
    Task<CheeseOption?> GetByIdAsync(int id);
}