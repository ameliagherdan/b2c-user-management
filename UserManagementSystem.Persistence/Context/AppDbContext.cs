using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Domain.Entities;

namespace UserManagementSystem.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}