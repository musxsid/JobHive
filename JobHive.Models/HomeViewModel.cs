using System.Collections.Generic;

namespace JobHive.Models
{
    public class HomeViewModel
    {
        public IEnumerable<CategoryCardVM> PopularCategories { get; set; } = new List<CategoryCardVM>();
        public IEnumerable<Job> FeaturedJobs { get; set; } = new List<Job>();
        public int TotalActiveJobs { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalApplicants { get; set; }
        public string? SelectedCategory { get; set; }
    }

    public class CategoryCardVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = "bi-briefcase";
        public string? Description { get; set; }
        public int JobCount { get; set; }
        public string BackgroundColorClass { get; set; } = "bg-primary-subtle";
        public string IconColorClass { get; set; } = "text-primary";
    }
}
