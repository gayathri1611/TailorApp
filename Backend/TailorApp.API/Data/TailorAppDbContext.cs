using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TailorApp.API.Models;

namespace TailorApp.API.Data;

public class TailorAppDbContext : IdentityDbContext<AppUser>
{
    public TailorAppDbContext(DbContextOptions<TailorAppDbContext> options)
        : base(options) { }

    public DbSet<Shop> Shops => Set<Shop>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Measurement> Measurements => Set<Measurement>();
    public DbSet<FabricInventory> FabricInventories => Set<FabricInventory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<NameValue> NameValues => Set<NameValue>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed message templates (existing)
        // Seed NameValues
        builder.Entity<NameValue>().HasData(
            // ── Units ──────────────────────────────────────
            new NameValue { NameValueId = 1, Category = "Unit", Value = "inch", Label = "Inch (\")", SortOrder = 1, IsActive = true },
            new NameValue { NameValueId = 2, Category = "Unit", Value = "cm", Label = "Centimeter (cm)", SortOrder = 2, IsActive = true },

            // ── Garment Types ───────────────────────────────
            new NameValue { NameValueId = 10, Category = "GarmentType", Value = "Shirt", SortOrder = 1, IsActive = true },
            new NameValue { NameValueId = 11, Category = "GarmentType", Value = "Pant", SortOrder = 2, IsActive = true },
            new NameValue { NameValueId = 12, Category = "GarmentType", Value = "Blouse", SortOrder = 3, IsActive = true },
            new NameValue { NameValueId = 13, Category = "GarmentType", Value = "Saree Fall", SortOrder = 4, IsActive = true },
            new NameValue { NameValueId = 14, Category = "GarmentType", Value = "Churidar", SortOrder = 5, IsActive = true },
            new NameValue { NameValueId = 15, Category = "GarmentType", Value = "Kurti", SortOrder = 6, IsActive = true },
            new NameValue { NameValueId = 16, Category = "GarmentType", Value = "Suit", SortOrder = 7, IsActive = true },
            new NameValue { NameValueId = 17, Category = "GarmentType", Value = "Coat", SortOrder = 8, IsActive = true },
            new NameValue { NameValueId = 18, Category = "GarmentType", Value = "Frock", SortOrder = 9, IsActive = true },
            new NameValue { NameValueId = 19, Category = "GarmentType", Value = "Lehenga", SortOrder = 10, IsActive = true },
            new NameValue { NameValueId = 20, Category = "GarmentType", Value = "Salwar", SortOrder = 11, IsActive = true },
            new NameValue { NameValueId = 21, Category = "GarmentType", Value = "Jacket", SortOrder = 12, IsActive = true },

            // ── Fabric Types ────────────────────────────────
            new NameValue { NameValueId = 30, Category = "FabricType", Value = "Cotton", SortOrder = 1, IsActive = true },
            new NameValue { NameValueId = 31, Category = "FabricType", Value = "Silk", SortOrder = 2, IsActive = true },
            new NameValue { NameValueId = 32, Category = "FabricType", Value = "Linen", SortOrder = 3, IsActive = true },
            new NameValue { NameValueId = 33, Category = "FabricType", Value = "Polyester", SortOrder = 4, IsActive = true },
            new NameValue { NameValueId = 34, Category = "FabricType", Value = "Chiffon", SortOrder = 5, IsActive = true },
            new NameValue { NameValueId = 35, Category = "FabricType", Value = "Georgette", SortOrder = 6, IsActive = true },
            new NameValue { NameValueId = 36, Category = "FabricType", Value = "Velvet", SortOrder = 7, IsActive = true },

            // Add these to the HasData() seed in TailorAppDbContext.OnModelCreating
            // after the existing NameValue seeds:

// ── Measurement Limits (min:max in inches) ──
new NameValue { NameValueId = 50, Category = "MeasurementLimit", Value = "chest:20:200", Label = "Chest (20–200)", SortOrder = 1, IsActive = true },
new NameValue { NameValueId = 51, Category = "MeasurementLimit", Value = "shoulder:10:100", Label = "Shoulder (10–100)", SortOrder = 2, IsActive = true },
new NameValue { NameValueId = 52, Category = "MeasurementLimit", Value = "sleevelength:10:150", Label = "Sleeve Length (10–150)", SortOrder = 3, IsActive = true },
new NameValue { NameValueId = 53, Category = "MeasurementLimit", Value = "armhole:10:100", Label = "Arm Hole (10–100)", SortOrder = 4, IsActive = true },
new NameValue { NameValueId = 54, Category = "MeasurementLimit", Value = "neck:10:80", Label = "Neck (10–80)", SortOrder = 5, IsActive = true },
new NameValue { NameValueId = 55, Category = "MeasurementLimit", Value = "waist:20:200", Label = "Waist (20–200)", SortOrder = 6, IsActive = true },
new NameValue { NameValueId = 56, Category = "MeasurementLimit", Value = "hip:20:200", Label = "Hip (20–200)", SortOrder = 7, IsActive = true },
new NameValue { NameValueId = 57, Category = "MeasurementLimit", Value = "thigh:10:150", Label = "Thigh (10–150)", SortOrder = 8, IsActive = true },
new NameValue { NameValueId = 58, Category = "MeasurementLimit", Value = "knee:10:120", Label = "Knee (10–120)", SortOrder = 9, IsActive = true },
new NameValue { NameValueId = 59, Category = "MeasurementLimit", Value = "inseamlength:10:200", Label = "Inseam (10–200)", SortOrder = 10, IsActive = true },
new NameValue { NameValueId = 60, Category = "MeasurementLimit", Value = "outseamlength:10:250", Label = "Outseam (10–250)", SortOrder = 11, IsActive = true },
new NameValue { NameValueId = 61, Category = "MeasurementLimit", Value = "height:30:300", Label = "Height (30–300)", SortOrder = 12, IsActive = true }
            );
    }
}