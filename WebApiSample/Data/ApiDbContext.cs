
using Microsoft.EntityFrameworkCore;
using WebApiSample.Models;

namespace WebApiSample.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    public DbSet<Store> Stores { get; set; }
}
