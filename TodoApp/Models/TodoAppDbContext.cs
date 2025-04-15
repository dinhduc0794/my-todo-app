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
}