using Globomantics.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Globomantics.Infrastructure.Data;

public class GlobomanticsContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<LineItem> LineItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder
            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GlobmanticsApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    public static void CreateInitialDatabase(GlobomanticsContext context)
    {
       var fisrtrun= context.Database.EnsureCreated();

        if (fisrtrun)
        {
            context.Database.Migrate();

            context.Products.Add(new Product { ProductId = Guid.Parse("4bc34cb4-c16e-4172-97af-4f90d2c494ec"), Name = "Alexander Lemtov Live", Price = 65m });
            context.Products.Add(new Product { ProductId = Guid.Parse("cda496ae-ec4d-410f-8bcd-26aaca5ba9da"), Name = "To The Moon And Back", Price = 135m });
            context.Products.Add(new Product { ProductId = Guid.Parse("92bc5f1c-0851-4fbb-931a-d6f807aae99a"), Name = "The State Of Affairs: Mariam Live!", Price = 85m });

            context.SaveChanges();
        }

    }
}
