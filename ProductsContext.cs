using System;
using Microsoft.EntityFrameworkCore;
using proiectPSSC.Data.DataModels;

namespace proiectPSSC.Data
{
	public class ProductsContext: DbContext
	{
        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
        }

        public DbSet<ProductDTO> Products { get; set; }

        public DbSet<ClientDTO> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientDTO>().ToTable("Client").HasKey(s => s.ClientId);
            modelBuilder.Entity<ProductDTO>().ToTable("Product").HasKey(s => s.ProductId);
        }
    }
}

