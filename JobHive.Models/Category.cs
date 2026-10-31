using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JobHive.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        [DisplayName("Category Name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        [DisplayName("Icon Class")]
        public string Icon { get; set; } = "bi-briefcase";

        [MaxLength(200)]
        [DisplayName("Description")]
        public string? Description { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }

        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
