using TailorApp.API.Models;

namespace TailorApp.API.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllActiveAsync();
    Task<Customer?> GetActiveByIdAsync(int id);
    Task<Customer?> FindAsync(int id);
    Task AddAsync(Customer customer);
    Task SaveChangesAsync();
}
