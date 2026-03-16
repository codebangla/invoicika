using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class InvoicikaDbContext : DbContext
    {
        public InvoicikaDbContext(DbContextOptions<InvoicikaDbContext> options)
            : base(options) {}

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public DbSet<CustomerInvoiceLine> CustomerInvoiceLines { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<ItemGroupItem> ItemGroupItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VAT> VATs { get; set; }
        public DbSet<CustomerInvoiceGroupLine> CustomerInvoiceGroupLines { get; set; }
        public DbSet<CustomerInvoiceGroupItemLine> CustomerInvoiceGroupItemLines { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerInvoiceLine>()
                .HasOne(cil => cil.Item)
                .WithMany(i => i.CustomerInvoiceLines)
                .HasForeignKey(cil => cil.Item_id)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete, keeps foreign key reference intact
        }

    }
}
