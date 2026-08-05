using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TailorApp.API.DTOs;
using TailorApp.API.Services;

namespace TailorApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        return Ok(await _customerService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
    {
        var customer = await _customerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCustomer(int id, UpdateCustomerDto dto)
    {
        if (!await _customerService.UpdateAsync(id, dto)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        if (!await _customerService.DeleteAsync(id)) return NotFound();
        return NoContent();
    }
}
