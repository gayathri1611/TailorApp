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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.IsActive)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderCode = o.OrderCode,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer!.FirstName + " " + (o.Customer.LastName ?? ""),
                ShopId = o.ShopId,
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
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
                    FabricDetails = i.FabricDetails,
                    SpecialInstructions = i.SpecialInstructions
                }).ToList()
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomer(int customerId)
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == customerId && o.IsActive)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderCode = o.OrderCode,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer!.FirstName + " " + (o.Customer.LastName ?? ""),
                ShopId = o.ShopId,
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
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
                    FabricDetails = i.FabricDetails,
                    SpecialInstructions = i.SpecialInstructions
                }).ToList()
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var o = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(o => o.OrderId == id && o.IsActive)
            .FirstOrDefaultAsync();

        if (o == null) return NotFound();

        return Ok(new OrderDto
        {
            OrderId = o.OrderId,
            OrderCode = o.OrderCode,
            CustomerId = o.CustomerId,
            CustomerName = o.Customer!.FirstName + " " + (o.Customer.LastName ?? ""),
            ShopId = o.ShopId,
            OrderDate = o.OrderDate,
            DeliveryDate = o.DeliveryDate,
            Status = o.Status,
            TotalAmount = o.TotalAmount,
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
                FabricDetails = i.FabricDetails,
                SpecialInstructions = i.SpecialInstructions
            }).ToList()
        });
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto dto)
    {
        var order = new Order
        {
            CustomerId = dto.CustomerId,
            ShopId = dto.ShopId,
            OrderDate = DateTime.UtcNow,
            DeliveryDate = dto.DeliveryDate,
            Status = "Pending",
            Notes = dto.Notes,
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
                FabricDetails = item.FabricDetails,
                SpecialInstructions = item.SpecialInstructions
            });
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.TotalPrice);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        order.OrderCode = $"ORD{order.OrderId:D5}";
        await _context.SaveChangesAsync();

        await _context.Entry(order).Reference(o => o.Customer).LoadAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, new OrderDto
        {
            OrderId = order.OrderId,
            OrderCode = order.OrderCode,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer != null
                ? order.Customer.FirstName + " " + (order.Customer.LastName ?? "")
                : "",
            ShopId = order.ShopId,
            OrderDate = order.OrderDate,
            DeliveryDate = order.DeliveryDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Notes = order.Notes,
            CreatedDate = order.CreatedDate,
            OrderItems = order.OrderItems.Select(i => new OrderItemDto
            {
                OrderItemId = i.OrderItemId,
                OrderId = i.OrderId,
                GarmentType = i.GarmentType,
                Description = i.Description,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                FabricDetails = i.FabricDetails,
                SpecialInstructions = i.SpecialInstructions
            }).ToList()
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(int id, UpdateOrderDto dto)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == id);

        if (order == null) return NotFound();

        order.DeliveryDate = dto.DeliveryDate;
        order.Status = dto.Status;
        order.Notes = dto.Notes;

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
                FabricDetails = item.FabricDetails,
                SpecialInstructions = item.SpecialInstructions
            });
        }

        order.TotalAmount = order.OrderItems.Sum(i => i.TotalPrice);
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
