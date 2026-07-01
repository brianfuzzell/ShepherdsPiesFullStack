using Microsoft.EntityFrameworkCore;
using ShepherdsPiesControllers.Data;
using ShepherdsPiesControllers.Models;

namespace ShepherdsPiesControllers.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ShepherdsPiesDbContext _context;

    public EmployeeRepository(ShepherdsPiesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
    }
}