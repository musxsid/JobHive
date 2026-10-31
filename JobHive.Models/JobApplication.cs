using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHive.Models
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int JobId { get; set; }

        [ForeignKey("JobId")]
        public virtual Job? Job { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(100)]
        [Display(Name = "Full Name")]
        public string ApplicantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        [MaxLength(100)]
        [Display(Name = "Email Address")]
        public string ApplicantEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [MaxLength(20)]
        [Display(Name = "Phone Number")]
        public string ApplicantPhone { get; set; } = string.Empty;

        [MaxLength(1000)]
        [Display(Name = "Cover Letter / Note")]
        public string? CoverLetter { get; set; }

        [MaxLength(255)]
        [Display(Name = "Resume File")]
        public string? ResumeFileName { get; set; }

        [MaxLength(255)]
        public string? ResumePath { get; set; }

        [Display(Name = "Application Date")]
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Applied"; // Applied, Under Review, Shortlisted, Interview, Offered, Rejected

        [MaxLength(500)]
        [Display(Name = "Recruiter Notes / Feedback")]
        public string? RecruiterNotes { get; set; }

        [NotMapped]
        public string StatusBadgeClass
        {
            get
            {
                return Status switch
                {
                    "Applied" => "bg-primary",
                    "Under Review" => "bg-info text-dark",
                    "Shortlisted" => "bg-warning text-dark",
                    "Interview" => "bg-secondary",
                    "Offered" => "bg-success",
                    "Rejected" => "bg-danger",
                    _ => "bg-secondary"
                };
            }
        }
    }
}
