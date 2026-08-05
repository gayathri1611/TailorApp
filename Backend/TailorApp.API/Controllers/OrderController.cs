using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorApp.API.DTOs;
using TailorApp.API.Services;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
    {
        return Ok(await _orderService.GetAllAsync());
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomer(int customerId)
    {
        return Ok(await _orderService.GetByCustomerAsync(customerId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrder(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder(CreateOrderDto dto)
    {
        var (orderId, orderCode) = await _orderService.CreateAsync(dto);
        return Ok(new { orderId, orderCode });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(int id, UpdateOrderDto dto)
    {
        if (!await _orderService.UpdateAsync(id, dto)) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
    {
        if (!await _orderService.UpdateStatusAsync(id, dto)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        if (!await _orderService.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}
