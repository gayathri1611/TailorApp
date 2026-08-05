using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly TailorAppDbContext _context;

    public OrderRepository(TailorAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetAllActiveAsync() =>
        await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Measurement)
            .Include(o => o.OrderItems).ThenInclude(i => i.Fabric)
            .Where(o => o.IsActive)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();

    public async Task<IEnumerable<Order>> GetActiveByCustomerAsync(int customerId) =>
        await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Measurement)
            .Include(o => o.OrderItems).ThenInclude(i => i.Fabric)
            .Where(o => o.CustomerId == customerId && o.IsActive)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();

    public async Task<Order?> GetActiveByIdWithDetailsAsync(int id) =>
        await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Measurement)
            .Include(o => o.OrderItems).ThenInclude(i => i.Fabric)
            .FirstOrDefaultAsync(o => o.OrderId == id && o.IsActive);

    public async Task<Order?> GetWithItemsByIdAsync(int id) =>
        await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);

    public async Task<Order?> FindAsync(int id) =>
        await _context.Orders.FindAsync(id);

    public Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<OrderItem> items)
    {
        _context.OrderItems.RemoveRange(items);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
