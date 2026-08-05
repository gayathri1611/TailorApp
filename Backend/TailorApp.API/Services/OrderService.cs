using TailorApp.API.DTOs;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;

    public OrderService(IOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _repo.GetAllActiveAsync();
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderDto>> GetByCustomerAsync(int customerId)
    {
        var orders = await _repo.GetActiveByCustomerAsync(customerId);
        return orders.Select(MapToDto);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _repo.GetActiveByIdWithDetailsAsync(id);
        return order == null ? null : MapToDto(order);
    }

    public async Task<(int orderId, string orderCode)> CreateAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            CustomerId = dto.CustomerId,
            ShopId = dto.ShopId,
            MeasurementId = dto.MeasurementId,
            OrderDate = DateTime.UtcNow,
            DeliveryDate = dto.DeliveryDate,
            Status = "Pending",
            Notes = dto.Notes,
            Discount = dto.Discount,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        foreach (var item in dto.OrderItems)
            order.OrderItems.Add(BuildOrderItem(item));

        CalculateTotals(order);

        await _repo.AddAsync(order);
        await _repo.SaveChangesAsync();

        order.OrderCode = $"ORD{order.OrderId:D5}";
        await _repo.SaveChangesAsync();

        return (order.OrderId, order.OrderCode);
    }

    public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto)
    {
        var order = await _repo.GetWithItemsByIdAsync(id);
        if (order == null) return false;

        order.MeasurementId = dto.MeasurementId;
        order.DeliveryDate = dto.DeliveryDate;
        order.Status = dto.Status;
        order.Notes = dto.Notes;
        order.Discount = dto.Discount;

        await _repo.RemoveRangeAsync(order.OrderItems);
        order.OrderItems.Clear();

        foreach (var item in dto.OrderItems)
            order.OrderItems.Add(BuildOrderItem(item));

        CalculateTotals(order);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
    {
        var order = await _repo.FindAsync(id);
        if (order == null) return false;

        order.Status = dto.Status;
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _repo.FindAsync(id);
        if (order == null) return false;

        order.IsActive = false;
        await _repo.SaveChangesAsync();
        return true;
    }

    private static OrderItem BuildOrderItem(CreateOrderItemDto item) => new()
    {
        GarmentType = item.GarmentType,
        Description = item.Description,
        Quantity = item.Quantity,
        UnitPrice = item.UnitPrice,
        TotalPrice = item.Quantity * item.UnitPrice,
        FabricId = item.FabricId,
        SpecialInstructions = item.SpecialInstructions
    };

    private static void CalculateTotals(Order order)
    {
        order.TotalAmount = order.OrderItems.Sum(i => i.TotalPrice);
        order.FinalAmount = Math.Max(0, order.TotalAmount - order.Discount);
    }

    private static OrderDto MapToDto(Order o) => new()
    {
        OrderId = o.OrderId,
        OrderCode = o.OrderCode,
        CustomerId = o.CustomerId,
        CustomerName = o.Customer != null ? o.Customer.FirstName + " " + (o.Customer.LastName ?? "") : "",
        CustomerPhone = o.Customer?.PhoneNumber,
        ShopId = o.ShopId,
        MeasurementId = o.MeasurementId,
        MeasurementCode = o.Measurement?.MeasurementCode,
        OrderDate = o.OrderDate,
        DeliveryDate = o.DeliveryDate,
        Status = o.Status,
        TotalAmount = o.TotalAmount,
        Discount = o.Discount,
        FinalAmount = o.FinalAmount,
        Notes = o.Notes,
        CreatedDate = o.CreatedDate,
        OrderItems = o.OrderItems.Select(i => new OrderItemDto
        {
            OrderItemId = i.OrderItemId,
            OrderId = i.OrderId,
            GarmentType = i.GarmentType,
            Description = i.Description,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice,
            TotalPrice = i.TotalPrice,
            FabricId = i.FabricId,
            FabricName = i.Fabric?.Name,
            SpecialInstructions = i.SpecialInstructions
        }).ToList()
    };
}
