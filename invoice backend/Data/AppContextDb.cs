using invoice_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace invoice_backend.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<User> Users { get; set; }
}