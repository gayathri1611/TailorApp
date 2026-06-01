using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.Data;
using TailorApp.API.DTOs;
using TailorApp.API.Models;

namespace TailorApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly TailorAppDbContext _context;

    public CustomersController(TailorAppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        var customers = await _context.Customers
            .Where(c => c.IsActive)
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                ShopId = c.ShopId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address
            })
            .ToListAsync();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var customer = await _context.Customers
            .Where(c => c.CustomerId == id && c.IsActive)
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                ShopId = c.ShopId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address
            })
            .FirstOrDefaultAsync();

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomer(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            ShopId = dto.ShopId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            Address = dto.Address,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return Ok(customer.CustomerId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCustomer(
        int id,
        UpdateCustomerDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound();

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.Email = dto.Email;
        customer.Address = dto.Address;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
            return NotFound();

        customer.IsActive = false;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}