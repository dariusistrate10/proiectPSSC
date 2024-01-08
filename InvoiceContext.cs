using System;
using Microsoft.EntityFrameworkCore;
using proiectPSSC.Data.DataModels;

namespace proiectPSSC.Data
{
    public class InvoiceContext : DbContext
    {
        public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options)
        {
        }

        public DbSet<InvoiceDTO> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvoiceDTO>().ToTable("Invoice").HasKey(i => i.InvoiceID);
        }
    }
}

