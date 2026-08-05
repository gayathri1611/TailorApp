using TailorApp.API.DTOs;
using TailorApp.API.Models;
using TailorApp.API.Repositories.Interfaces;

namespace TailorApp.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerService(ICustomerRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _repo.GetAllActiveAsync();
        return customers.Select(MapToDto);
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _repo.GetActiveByIdAsync(id);
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
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

        await _repo.AddAsync(customer);
        await _repo.SaveChangesAsync();

        customer.CustomerCode = $"CUST{customer.CustomerId:D5}";
        await _repo.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _repo.FindAsync(id);
        if (customer == null) return false;

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.Email = dto.Email;
        customer.Address = dto.Address;

        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _repo.FindAsync(id);
        if (customer == null) return false;

        customer.IsActive = false;
        await _repo.SaveChangesAsync();
        return true;
    }

    private static CustomerDto MapToDto(Customer c) => new()
    {
        CustomerId = c.CustomerId,
        CustomerCode = c.CustomerCode,
        ShopId = c.ShopId,
        FirstName = c.FirstName,
        LastName = c.LastName,
        PhoneNumber = c.PhoneNumber,
        Email = c.Email,
        Address = c.Address
    };
}
