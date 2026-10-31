using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHive.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Job title is required")]
        [MaxLength(100)]
        [Display(Name = "Job Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(100)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; } = string.Empty;

        [MaxLength(255)]
        [Display(Name = "Company Logo / Image URL")]
        public string? CompanyLogo { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Remote Work Available")]
        public bool IsRemote { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Job Type")]
        public string JobType { get; set; } = "Full-Time"; // Full-Time, Part-Time, Remote, Contract, Internship

        [Required]
        [MaxLength(50)]
        [Display(Name = "Experience Level")]
        public string ExperienceLevel { get; set; } = "Mid Level"; // Entry Level, Mid Level, Senior Level, Lead

        [Required]
        [Range(0, 100000000, ErrorMessage = "Minimum salary must be non-negative")]
        [Display(Name = "Minimum Salary (₹ or $)")]
        public decimal SalaryMin { get; set; }

        [Required]
        [Range(0, 100000000, ErrorMessage = "Maximum salary must be non-negative")]
        [Display(Name = "Maximum Salary (₹ or $)")]
        public decimal SalaryMax { get; set; }

        [Required(ErrorMessage = "Job description is required")]
        public string Description { get; set; } = string.Empty;

        public string? Requirements { get; set; }

        public string? Responsibilities { get; set; }

        public string? Benefits { get; set; }

        [Required]
        [Display(Name = "Job Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Display(Name = "Date Posted")]
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Application Deadline")]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Active Listing")]
        public bool IsActive { get; set; } = true;

        [MaxLength(100)]
        [EmailAddress]
        [Display(Name = "Recruiter / Employer Email")]
        public string EmployerEmail { get; set; } = "recruiter@jobhive.com";

        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

        [NotMapped]
        public string FormattedSalary
        {
            get
            {
                if (SalaryMin > 0 && SalaryMax > 0)
                {
                    if (SalaryMax >= 100000)
                    {
                        return $"₹{SalaryMin / 100000:0.#}L - ₹{SalaryMax / 100000:0.#}L / yr";
                    }
                    return $"${SalaryMin:N0} - ${SalaryMax:N0} / yr";
                }
                else if (SalaryMin > 0)
                {
                    return $"From ₹{SalaryMin:N0} / yr";
                }
                return "Competitive Compensation";
            }
        }
    }
}
