using System.ComponentModel.DataAnnotations;

namespace ContactProMVC.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? AppUserId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        // Navigation Properties
        public virtual AppUser? AppUser { get; set; }
    }
}