using Microsoft.EntityFrameworkCore;
using OrderWorkflow.Models;

namespace OrderWorkflow
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurarea tabelei Product
            modelBuilder.Entity<Product>(entityBuilder =>
            {
                entityBuilder.ToTable("Product");
                entityBuilder.HasKey(p => p.ProductId);
                entityBuilder.Property(p => p.Code).IsRequired().HasMaxLength(50);
                entityBuilder.Property(p => p.Stoc).IsRequired();
            });

            // Configurarea tabelei OrderHeader
            modelBuilder.Entity<OrderHeader>(entityBuilder =>
            {
                entityBuilder.ToTable("OrderHeader");
                entityBuilder.HasKey(o => o.OrderId);
                entityBuilder.Property(o => o.Address).IsRequired().HasMaxLength(255);
                entityBuilder.Property(o => o.Total).HasColumnType("decimal(10, 2)").IsRequired();

                // Relație one-to-many între OrderHeader și OrderLine
                entityBuilder.HasMany(o => o.OrderLines)
                             .WithOne(ol => ol.OrderHeader)
                             .HasForeignKey(ol => ol.OrderId)
                             .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurarea tabelei OrderLine
            modelBuilder.Entity<OrderLine>(entityBuilder =>
            {
                entityBuilder.ToTable("OrderLine");
                entityBuilder.HasKey(ol => ol.OrderLineId);
                entityBuilder.Property(ol => ol.Quantity).IsRequired();
                entityBuilder.Property(ol => ol.Price).HasColumnType("decimal(10, 2)").IsRequired();

                // Relația dintre OrderLine și Product
                entityBuilder.HasOne(ol => ol.Product)
                             .WithMany()
                             .HasForeignKey(ol => ol.ProductId)
                             .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
