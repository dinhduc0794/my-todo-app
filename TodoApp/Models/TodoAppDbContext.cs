using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace TodoApp.Models;

public class TodoAppDbContext : DbContext
{
    public DbSet<Task> tasks { get; set; }
    public DbSet<Category> categories { get; set; }
    
    private const string connectionString = @"
        Server=localhost;
        Port=3306;
        Database=todoapp;
        User=root;
        Password=2210794;
    ";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var serverVersion = new MySqlServerVersion(new Version(9, 0, 0)); 

        optionsBuilder.UseMySql(connectionString, serverVersion);
    }
    
    public override int SaveChanges()
    {
        UpdateBaseModelTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateBaseModelTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateBaseModelTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseModel && 
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseModel)entry.Entity;
            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

}