using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<ProductPromotion> ProductPromotions { get; set; }

    public DbSet<Category> Categories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<ProductPromotion>()
        .HasKey(pp => new { pp.ProductId, pp.PromotionId });

    modelBuilder.Entity<ProductPromotion>()
        .HasOne(pp => pp.Product)
        .WithMany(p => p.ProductPromotions)
        .HasForeignKey(pp => pp.ProductId);

    modelBuilder.Entity<ProductPromotion>()
        .HasOne(pp => pp.Promotion)
        .WithMany(p => p.ProductPromotions)
        .HasForeignKey(pp => pp.PromotionId);

    // Пример начальных данных:
    modelBuilder.Entity<Promotion>().HasData(
        new Promotion { Id = 1, Title = "Весняна знижка", DiscountPercentage = 10, StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(10) }
    );
 
    modelBuilder.Entity<Product>().HasData(
        new Product { Id = 1, Name = "Кіт", Description = "Іграшковий кіт", Price = 300 }
    );

    modelBuilder.Entity<ProductPromotion>().HasData(
        new ProductPromotion { ProductId = 1, PromotionId = 1 }
    );
}


}

