using TailorApp.API.Models;

namespace TailorApp.API.Repositories.Interfaces;

public interface IMeasurementRepository
{
    Task<IEnumerable<Measurement>> GetAllActiveAsync();
    Task<IEnumerable<Measurement>> GetActiveByCustomerAsync(int customerId);
    Task<Measurement?> GetActiveByIdAsync(int id);
    Task<Measurement?> FindAsync(int id);
    Task AddAsync(Measurement measurement);
    Task LoadCustomerAsync(Measurement measurement);
    Task SaveChangesAsync();
}
