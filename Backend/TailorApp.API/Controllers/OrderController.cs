using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.DTOs;
using TailorApp.API.Models;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly TailorAppDbContext _context;

    public OrdersController(TailorAppDbContext context)
    {
        _context = context;
    }

    private static OrderDto MapToDto(Order o) => new()
    {
        OrderId = o.OrderId,
        OrderCode = o.OrderCode,
        CustomerId = o.CustomerId,
        CustomerName = o.Customer != null
                            ? o.Customer.FirstName + " " + (o.Customer.LastName ?? "")
                            : "",
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Measurement)
            .Include(o => o.OrderItems).ThenInclude(i => i.Fabric)
            .Where(o => o.IsActive)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();

        return Ok(orders.Select(MapToDto));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomer(int customerId)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Measurement)
            .Include(o => o.OrderItems).ThenInclude(i => i.Fabric)
            .Where(o => o.CustomerId == customerId && o.IsActive)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();

        return Ok(orders.Select(MapToDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var o = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Measurement)
            .Include(o => o.OrderItems).ThenInclude(i => i.Fabric)
            .Where(o => o.OrderId == id && o.IsActive)
            .FirstOrDefaultAsync();

        if (o == null) return NotFound();
        return Ok(MapToDto(o));
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder(CreateOrderDto dto)
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
        {
            order.OrderItems.Add(new OrderItem
            {
                GarmentType = item.GarmentType,
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice,
                FabricId = item.FabricId,
                SpecialInstructions = item.SpecialInstructions
            });
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.TotalPrice);
        order.FinalAmount = Math.Max(0, order.TotalAmount - order.Discount);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        order.OrderCode = $"ORD{order.OrderId:D5}";
        await _context.SaveChangesAsync();

        return Ok(new { order.OrderId, order.OrderCode });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(int id, UpdateOrderDto dto)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null) return NotFound();

        order.MeasurementId = dto.MeasurementId;
        order.DeliveryDate = dto.DeliveryDate;
        order.Status = dto.Status;
        order.Notes = dto.Notes;
        order.Discount = dto.Discount;

        // Replace order items
        _context.OrderItems.RemoveRange(order.OrderItems);
        order.OrderItems.Clear();

        foreach (var item in dto.OrderItems)
        {
            order.OrderItems.Add(new OrderItem
            {
                GarmentType = item.GarmentType,
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice,
                FabricId = item.FabricId,
                SpecialInstructions = item.SpecialInstructions
            });
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.TotalPrice);
        order.FinalAmount = Math.Max(0, order.TotalAmount - order.Discount);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        order.Status = dto.Status;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        order.IsActive = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}