using Microsoft.EntityFrameworkCore;
using TailorApp.API.Models;

namespace TailorApp.API.Data;

public class TailorAppDbContext : DbContext
{
    public TailorAppDbContext(
        DbContextOptions<TailorAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Shop> Shops => Set<Shop>();

    public DbSet<Customer> Customers => Set<Customer>();
}