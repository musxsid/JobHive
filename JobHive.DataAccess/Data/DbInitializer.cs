using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace JobHive.DataAccess.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                // Ensure database and tables are created with seed data
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                // In environments without active LocalDB/SQL Server instances, log or handle gracefully
                Console.WriteLine($"Database initialization warning: {ex.Message}");
            }
        }
    }
}

