using JobHive.DataAccess.Data;
using JobHive.DataAccess.Repository;
using JobHive.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobHive.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Cross-Platform Database Configuration (SQL Server / SQLite)
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
            var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? 
                (connectionString.Contains(".db") || (connectionString.Contains("Data Source=") && !connectionString.Contains("Server=")) ? "Sqlite" : "SqlServer");

            if (dbProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(connectionString));
            }
            else
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString, sqlOptions =>
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null)));
            }

            // Unit of Work Dependency Injection
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Session support for role-switching and candidate simulation
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Auto-initialize / seed database tables
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogWarning(ex, "Could not automatically initialize SQL Server database during startup.");
                }
            }

            // Configure HTTP pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Applicant}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
