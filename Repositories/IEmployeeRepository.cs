using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(string id);
}