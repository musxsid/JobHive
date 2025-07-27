using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobHive.Models
{
    public class Job
    {
        [Key]
            public int Id { get; set; } 

        [Required]
            public string Title { get; set; }

        [Required]
            public string CompanyName { get; set; }// Job title (e.g., "Software Engineer")
        [Required]
            public string Description { get; set; }  // Job details
        [Required]
            public string Location { get; set; }     // Office location

            public double Salary { get; set; }       // Salary offered
        }
    }

