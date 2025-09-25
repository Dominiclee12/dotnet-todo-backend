using WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Todo> Todos { get; set; }
}
