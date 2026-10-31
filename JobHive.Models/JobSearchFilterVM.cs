using System.Collections.Generic;

namespace JobHive.Models
{
    public class JobSearchFilterVM
    {
        public string? SearchKeyword { get; set; }
        public string? Location { get; set; }
        public int? CategoryId { get; set; }
        public string? JobType { get; set; }
        public string? ExperienceLevel { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? SortBy { get; set; } = "date_desc"; // date_desc, salary_desc, salary_asc, title_asc

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalJobs { get; set; }
        public int TotalPages => (TotalJobs + PageSize - 1) / (PageSize > 0 ? PageSize : 1);

        public IEnumerable<Job> Jobs { get; set; } = new List<Job>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

        public Dictionary<string, int> JobTypeCounts { get; set; } = new();
        public Dictionary<string, int> ExperienceCounts { get; set; } = new();
    }
}

