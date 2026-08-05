using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly TailorAppDbContext _context;

    public CustomerRepository(TailorAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllActiveAsync() =>
        await _context.Customers.Where(c => c.IsActive).ToListAsync();

    public async Task<Customer?> GetActiveByIdAsync(int id) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id && c.IsActive);

    public async Task<Customer?> FindAsync(int id) =>
        await _context.Customers.FindAsync(id);

    public Task AddAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
