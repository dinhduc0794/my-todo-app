using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp;

public class Program
{
    static void CreateDatabase()
    {   
        // using (resource management) -> doi tuong context se tu dong giai phong tai nguyen khi ra khoi block
        using (var context = new TodoAppDbContext())
        {
            context.Database.EnsureCreated();
            string dbName = context.Database.GetDbConnection().Database;
            Console.WriteLine($"Database '{dbName}' has been created.");

            var result = context.Database.EnsureCreated();  // kiem tra tren server db da ton tai hay chua, neu chua thi tao -> tra ve boolean
            
            if (result)
            {
                Console.WriteLine($"Database '{dbName}' has been created.");
            }
            else
            {
                Console.WriteLine($"Database '{dbName}' already exists.");
            }
        }
    }
    static void DropDatabase()
    {   
        // using (resource management) -> doi tuong context se tu dong giai phong tai nguyen khi ra khoi block
        using (var context = new TodoAppDbContext())
        {
            context.Database.EnsureDeleted();
            string dbName = context.Database.GetDbConnection().Database;
            Console.WriteLine($"Database '{dbName}' has been deleted.");

            var result = context.Database.EnsureCreated();  // xoa db -< tra ve true neu xoa thanh cong
            
            if (result)
            {
                Console.WriteLine($"Database '{dbName}' has been deleted.");
            }
            else
            {
                Console.WriteLine($"Cannot drop atabase '{dbName}'.");
            }
        }
    }
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        CreateDatabase();
        
        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}