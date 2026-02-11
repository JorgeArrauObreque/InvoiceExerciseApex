using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace InvoiceExercise.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<CreditNote> CreditNotes { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // facturas
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.InvoiceNumber);
                entity.Property(e => e.InvoiceNumber).IsRequired();
                entity.HasIndex(e => e.InvoiceNumber).IsUnique();

                entity.Property(e => e.InvoiceDate).IsRequired();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.PaymentDueDate).IsRequired();

                entity.Property(e => e.CustomerRun).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(200);

                entity.Property(e => e.IsConsistent).IsRequired();

                // FKs
                entity.HasMany(e => e.Items)
                    .WithOne(e => e.Invoice)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.CreditNotes)
                    .WithOne(e => e.Invoice)
                    .HasForeignKey(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Payment)
                    .WithOne(e => e.Invoice)
                    .HasForeignKey<Payment>(e => e.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración del detalle
            modelBuilder.Entity<InvoiceItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProductName).IsRequired().HasMaxLength(300);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)").IsRequired();
            });

            // Configuración de las notas de crédito
            modelBuilder.Entity<CreditNote>(entity =>
            {
                entity.HasKey(e => e.CreditNoteNumber);
                entity.Property(e => e.CreditNoteNumber).IsRequired();
                entity.Property(e => e.CreditNoteDate).IsRequired();
                entity.Property(e => e.CreditNoteAmount).HasColumnType("decimal(18,2)").IsRequired();

                entity.HasIndex(e => e.CreditNoteNumber);
            });

            // Configuración de los pagos
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PaymentMethod).HasMaxLength(100);
                entity.Property(e => e.PaymentDate);

                entity.HasIndex(e => e.InvoiceId).IsUnique();
            });
        }
    }
}
