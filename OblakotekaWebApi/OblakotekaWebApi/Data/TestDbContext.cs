using Microsoft.EntityFrameworkCore;
using OblakotekaWebApi.Models;

namespace OblakotekaWebApi;
public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }
}