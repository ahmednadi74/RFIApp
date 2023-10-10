using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

public class SupplierContext : IdentityDbContext
{
    public SupplierContext(DbContextOptions<SupplierContext> options)
        : base(options)
    {
    }

    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<BankDetails> BankDetails { get; set; }
    public DbSet<ProductionInformation> ProductionInformation { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProductionInformation>()
        .Property(p => p.ExportCapacity)
        .HasColumnType("decimal(18,2)");


        modelBuilder.Entity<ProductionInformation>()
       .Property(p => p.ProductionCapacity)
       .HasColumnType("decimal(18,2)");

        

        modelBuilder.Entity<Supplier>()
           .HasMany(s => s.BankDetails)
           .WithOne(b => b.Supplier)
           .HasForeignKey(b => b.SupplierID);

        modelBuilder.Entity<Supplier>()
            .HasMany(s => s.Contacts)
            .WithOne(c => c.Supplier)
            .HasForeignKey(c => c.SupplierID);

        modelBuilder.Entity<Supplier>()
            .HasMany(s => s.ProductionInformation)
            .WithOne(p => p.Supplier)
            .HasForeignKey(p => p.SupplierID);

    }
}