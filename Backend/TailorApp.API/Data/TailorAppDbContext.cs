using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.Models;

namespace TailorApp.API.Data;

public class TailorAppDbContext : IdentityDbContext<AppUser>
{
    public TailorAppDbContext(
        DbContextOptions<TailorAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<Customer> Customers => Set<Customer>();

    // Phase 1
    public DbSet<Measurement> Measurements => Set<Measurement>();

    // Phase 2
    public DbSet<FabricInventory> FabricInventories => Set<FabricInventory>();

    // Phase 3
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}