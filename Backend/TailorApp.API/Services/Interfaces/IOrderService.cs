using TailorApp.API.DTOs;

namespace TailorApp.API.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<IEnumerable<OrderDto>> GetByCustomerAsync(int customerId);
    Task<OrderDto?> GetByIdAsync(int id);
    Task<(int orderId, string orderCode)> CreateAsync(CreateOrderDto dto);
    Task<bool> UpdateAsync(int id, UpdateOrderDto dto);
    Task<bool> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
    Task<bool> DeleteAsync(int id);
}
