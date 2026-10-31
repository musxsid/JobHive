using System;
using System.ComponentModel.DataAnnotations;

namespace JobHive.Models
{
    public class CandidateProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Headline { get; set; } // e.g. "Software Developer (.NET / C#)"

        [MaxLength(100)]
        public string? Location { get; set; }

        [MaxLength(300)]
        public string? Skills { get; set; } // comma separated

        [Range(0, 50)]
        public int ExperienceYears { get; set; }

        [MaxLength(255)]
        public string? ResumeFileName { get; set; }

        [MaxLength(255)]
        public string? ResumePath { get; set; }

        [MaxLength(1000)]
        public string? Bio { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
