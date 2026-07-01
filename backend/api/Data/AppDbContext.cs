using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("members");
            entity.HasKey(e => e.MemberId);
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(50);
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(50);
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(e => e.ProductId);
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName).HasColumnName("product_name").HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(10,2)");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.IsAvailable).HasColumnName("is_available");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ShippingAddress).HasColumnName("shipping_address").HasMaxLength(255);
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(12,2)");
            entity.Property(e => e.IsPaid).HasColumnName("is_paid");
            entity.Property(e => e.OrderDate).HasColumnName("order_date");

            entity.HasOne(e => e.Member)
                  .WithMany(m => m.Orders)
                  .HasForeignKey(e => e.MemberId)
                  .HasConstraintName("fk_orders_member");

            entity.HasOne(e => e.Product)
                  .WithMany(p => p.Orders)
                  .HasForeignKey(e => e.ProductId)
                  .HasConstraintName("fk_orders_product");
        });
    }
}
