using JobHive.Models;
using Microsoft.EntityFrameworkCore;


namespace JobHive.DataAccess.Data
{
    public class ApplicationDbContext : DbContext {
        internal readonly object Categories;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Category> JobCategory { get; set; }
        public DbSet<Job> Jobs{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Computer Science", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Information Technology (IT)", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Software Development", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Data Science & Analytics", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Artificial Intelligence (AI)", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Cybersecurity", DisplayOrder = 6 },
                new Category { Id = 7, Name = "Cloud Computing", DisplayOrder = 7 },
                new Category { Id = 8, Name = "Web Development", DisplayOrder = 8 },
                new Category { Id = 9, Name = "Mobile App Development", DisplayOrder = 9 },
                new Category { Id = 10, Name = "DevOps & SysAdmin", DisplayOrder = 10 },
                new Category { Id = 11, Name = "Networking", DisplayOrder = 11 },
                new Category { Id = 12, Name = "Database Administration", DisplayOrder = 12 },
                new Category { Id = 13, Name = "UI/UX Design", DisplayOrder = 13 },
                new Category { Id = 14, Name = "Quality Assurance (QA)", DisplayOrder = 14 },
                new Category { Id = 15, Name = "Game Development", DisplayOrder = 15 },
                new Category { Id = 16, Name = "Embedded Systems", DisplayOrder = 16 },
                new Category { Id = 17, Name = "Blockchain", DisplayOrder = 17 },
                new Category { Id = 18, Name = "Finance & Banking", DisplayOrder = 18 },
                new Category { Id = 19, Name = "Healthcare", DisplayOrder = 19 },
                new Category { Id = 20, Name = "Education & Training", DisplayOrder = 20 },
                new Category { Id = 21, Name = "Marketing & Advertising", DisplayOrder = 21 },
                new Category { Id = 22, Name = "Sales & Business Development", DisplayOrder = 22 },
                new Category { Id = 23, Name = "Customer Support", DisplayOrder = 23 },
                new Category { Id = 24, Name = "Human Resources (HR)", DisplayOrder = 24 },
                new Category { Id = 25, Name = "Accounting & Auditing", DisplayOrder = 25 },
                new Category { Id = 26, Name = "Legal", DisplayOrder = 26 },
                new Category { Id = 27, Name = "Engineering (Non-IT)", DisplayOrder = 27 },
                new Category { Id = 28, Name = "Manufacturing", DisplayOrder = 28 },
                new Category { Id = 29, Name = "Retail", DisplayOrder = 29 },
                new Category { Id = 30, Name = "Hospitality & Tourism", DisplayOrder = 30 },
                new Category { Id = 31, Name = "Media & Entertainment", DisplayOrder = 31 },
                new Category { Id = 32, Name = "Writing & Content Creation", DisplayOrder = 32 },
                new Category { Id = 33, Name = "Architecture & Design", DisplayOrder = 33 },
                new Category { Id = 34, Name = "Logistics & Supply Chain", DisplayOrder = 34 },
                new Category { Id = 35, Name = "Research & Development", DisplayOrder = 35 },
                new Category { Id = 36, Name = "Government & Public Sector", DisplayOrder = 36 },
                new Category { Id = 37, Name = "Non-Profit & Social Services", DisplayOrder = 37 },
                new Category { Id = 38, Name = "Environmental & Sustainability", DisplayOrder = 38 },
                new Category { Id = 39, Name = "Agriculture & Forestry", DisplayOrder = 39 },
                new Category { Id = 40, Name = "Other", DisplayOrder = 40 }
                );

              modelBuilder.Entity<Job>().HasData(
                new Job { Id = 1, Title = "Software Engineer", CompanyName = "Tech Solutions", Description = "Develop and maintain software applications.", Location = "Bangalore", Salary = 80000 },
                new Job { Id = 2, Title = "Data Analyst", CompanyName = "Data Insights", Description = "Analyze data to provide business insights.", Location = "Pune", Salary = 70000 },
                new Job { Id = 3, Title = "Web Developer", CompanyName = "Web Creators", Description = "Build and maintain websites.", Location = "Delhi", Salary = 75000 },
                new Job { Id = 4, Title = "Cybersecurity Specialist", CompanyName = "SecureTech", Description = "Protect systems and networks from cyber threats.", Location = "Hyderabad", Salary = 90000 },
                new Job { Id = 5, Title = "Cloud Engineer", CompanyName = "Cloud Innovators", Description = "Design and manage cloud infrastructure.", Location = "Chennai", Salary = 85000 }
            
               );

        }
        
          
        

    }
}
