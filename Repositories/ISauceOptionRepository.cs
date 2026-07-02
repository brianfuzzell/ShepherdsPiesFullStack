using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public interface ISauceOptionRepository
{
    Task<List<SauceOption>> GetAllAsync();
    Task<SauceOption?> GetByIdAsync(int id);
}