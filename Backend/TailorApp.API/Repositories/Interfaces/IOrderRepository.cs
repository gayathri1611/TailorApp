using TailorApp.API.Models;

namespace TailorApp.API.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllActiveAsync();
    Task<IEnumerable<Order>> GetActiveByCustomerAsync(int customerId);
    Task<Order?> GetActiveByIdWithDetailsAsync(int id);
    Task<Order?> GetWithItemsByIdAsync(int id);
    Task<Order?> FindAsync(int id);
    Task AddAsync(Order order);
    Task RemoveRangeAsync(IEnumerable<OrderItem> items);
    Task SaveChangesAsync();
}
