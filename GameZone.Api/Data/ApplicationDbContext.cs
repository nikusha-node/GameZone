using GameZone.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductSpecification> ProductSpecifications => Set<ProductSpecification>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<DigitalDelivery> DigitalDeliveries => Set<DigitalDelivery>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>().HasIndex(c => c.Slug).IsUnique();
        builder.Entity<Brand>().HasIndex(b => b.Slug).IsUnique();
        builder.Entity<Product>().HasIndex(p => p.Slug).IsUnique();
        builder.Entity<Product>().HasIndex(p => p.Sku).IsUnique();
        builder.Entity<Order>().HasIndex(o => o.OrderNumber).IsUnique();

        builder.Entity<CartItem>().HasIndex(ci => new { ci.CartId, ci.ProductId }).IsUnique();
        builder.Entity<Review>().HasIndex(r => new { r.UserId, r.ProductId }).IsUnique();
        builder.Entity<WishlistItem>().HasIndex(w => new { w.UserId, w.ProductId }).IsUnique();

        builder.Entity<Product>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_Product_Price", "[Price] >= 0");
            t.HasCheckConstraint("CK_Product_Stock", "[StockQuantity] >= 0");
        });
        builder.Entity<Review>().ToTable(t =>
            t.HasCheckConstraint("CK_Review_Rating", "[Rating] BETWEEN 1 AND 5"));
        builder.Entity<CartItem>().ToTable(t =>
            t.HasCheckConstraint("CK_CartItem_Quantity", "[Quantity] > 0"));
        builder.Entity<OrderItem>().ToTable(t =>
            t.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0"));


        builder.Entity<Product>()
            .HasOne(p => p.Category).WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
            .HasOne(p => p.Brand).WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<ProductImage>()
            .HasOne(i => i.Product).WithMany(p => p.Images)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProductSpecification>()
            .HasOne(s => s.Product).WithMany(p => p.Specifications)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Game>()
            .HasOne(g => g.Product).WithOne(p => p.Game)
            .HasForeignKey<Game>(g => g.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Game>()
            .HasOne(g => g.Platform).WithMany(p => p.Games)
            .HasForeignKey(g => g.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Cart>()
            .HasOne(c => c.User).WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CartItem>()
            .HasOne(ci => ci.Cart).WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CartItem>()
            .HasOne(ci => ci.Product).WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .HasOne(o => o.User).WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Order).WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product).WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DigitalDelivery>()
            .HasOne(d => d.OrderItem).WithOne(oi => oi.DigitalDelivery)
            .HasForeignKey<DigitalDelivery>(d => d.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Review>()
            .HasOne(r => r.Product).WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Review>()
            .HasOne(r => r.User).WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WishlistItem>()
            .HasOne(w => w.User).WithMany(u => u.WishlistItems)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WishlistItem>()
            .HasOne(w => w.Product).WithMany()
            .HasForeignKey(w => w.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}