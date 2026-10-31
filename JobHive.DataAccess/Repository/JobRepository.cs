using JobHive.DataAccess.Data;
using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobHive.DataAccess.Repository
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        private readonly ApplicationDbContext _db;

        public JobRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Job obj)
        {
            var objFromDb = _db.Jobs.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.CompanyName = obj.CompanyName;
                objFromDb.CompanyLogo = obj.CompanyLogo;
                objFromDb.Location = obj.Location;
                objFromDb.IsRemote = obj.IsRemote;
                objFromDb.JobType = obj.JobType;
                objFromDb.ExperienceLevel = obj.ExperienceLevel;
                objFromDb.SalaryMin = obj.SalaryMin;
                objFromDb.SalaryMax = obj.SalaryMax;
                objFromDb.Description = obj.Description;
                objFromDb.Requirements = obj.Requirements;
                objFromDb.Responsibilities = obj.Responsibilities;
                objFromDb.Benefits = obj.Benefits;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Deadline = obj.Deadline;
                objFromDb.IsActive = obj.IsActive;
                objFromDb.EmployerEmail = obj.EmployerEmail;
            }
        }

        public IEnumerable<Job> GetFeaturedJobs(int count = 6)
        {
            return _db.Jobs
                .Include(j => j.Category)
                .Where(j => j.IsActive)
                .OrderByDescending(j => j.PostedDate)
                .Take(count)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Compound database query in Entity Framework Core to evaluate criteria concurrently,
        /// optimizing SQL execution time and reducing search latency.
        /// </summary>
        public JobSearchFilterVM SearchJobs(JobSearchFilterVM filter)
        {
            IQueryable<Job> query = _db.Jobs
                .Include(j => j.Category)
                .Where(j => j.IsActive);

            // 1. Keyword search (Title, Company, Description, Requirements)
            if (!string.IsNullOrWhiteSpace(filter.SearchKeyword))
            {
                var term = filter.SearchKeyword.Trim();
                query = query.Where(j =>
                    EF.Functions.Like(j.Title, $"%{term}%") ||
                    EF.Functions.Like(j.CompanyName, $"%{term}%") ||
                    EF.Functions.Like(j.Description, $"%{term}%") ||
                    (j.Requirements != null && EF.Functions.Like(j.Requirements, $"%{term}%")));
            }

            // 2. Location / Region filter
            if (!string.IsNullOrWhiteSpace(filter.Location))
            {
                var loc = filter.Location.Trim();
                if (loc.Equals("Remote", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(j => j.IsRemote || EF.Functions.Like(j.Location, "%Remote%"));
                }
                else
                {
                    query = query.Where(j => EF.Functions.Like(j.Location, $"%{loc}%"));
                }
            }

            // 3. Category filter
            if (filter.CategoryId.HasValue && filter.CategoryId.Value > 0)
            {
                query = query.Where(j => j.CategoryId == filter.CategoryId.Value);
            }

            // 4. Job Type filter
            if (!string.IsNullOrWhiteSpace(filter.JobType) && !filter.JobType.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(j => j.JobType == filter.JobType);
            }

            // 5. Experience Level filter
            if (!string.IsNullOrWhiteSpace(filter.ExperienceLevel) && !filter.ExperienceLevel.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(j => j.ExperienceLevel == filter.ExperienceLevel);
            }

            // 6. Minimum Salary filter
            if (filter.MinSalary.HasValue && filter.MinSalary.Value > 0)
            {
                query = query.Where(j => j.SalaryMax >= filter.MinSalary.Value || j.SalaryMin >= filter.MinSalary.Value);
            }

            // 7. Maximum Salary filter
            if (filter.MaxSalary.HasValue && filter.MaxSalary.Value > 0)
            {
                query = query.Where(j => j.SalaryMin <= filter.MaxSalary.Value);
            }

            // Count total matching records before pagination
            filter.TotalJobs = query.Count();

            // 8. Compound Sorting
            query = filter.SortBy switch
            {
                "salary_desc" => query.OrderByDescending(j => j.SalaryMax).ThenByDescending(j => j.PostedDate),
                "salary_asc" => query.OrderBy(j => j.SalaryMin).ThenByDescending(j => j.PostedDate),
                "title_asc" => query.OrderBy(j => j.Title),
                _ => query.OrderByDescending(j => j.PostedDate)
            };

            // 9. Pagination
            if (filter.PageNumber < 1) filter.PageNumber = 1;
            if (filter.PageSize < 1) filter.PageSize = 9;

            filter.Jobs = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .AsNoTracking()
                .ToList();

            // Populate metadata for filter badges
            filter.Categories = _db.Categories.OrderBy(c => c.DisplayOrder).AsNoTracking().ToList();

            // Populate counts
            filter.JobTypeCounts = _db.Jobs
                .Where(j => j.IsActive)
                .GroupBy(j => j.JobType)
                .ToDictionary(g => g.Key, g => g.Count());

            filter.ExperienceCounts = _db.Jobs
                .Where(j => j.IsActive)
                .GroupBy(j => j.ExperienceLevel)
                .ToDictionary(g => g.Key, g => g.Count());

            return filter;
        }
    }
}
