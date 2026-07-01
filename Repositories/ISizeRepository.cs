using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public interface ISizeRepository
{
    Task<List<Size>> GetAllAsync();
    Task<Size?> GetByIdAsync(int id);
}