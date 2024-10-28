using ApiWeb.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Data;

public class MyDBContext : DbContext
{
    public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }
}
