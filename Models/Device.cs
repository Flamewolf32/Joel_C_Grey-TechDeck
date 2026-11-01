using System.ComponentModel.DataAnnotations;

namespace TechDesk.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The asset tag is required (e.g., ECC-0042).")]
        [StringLength(10, ErrorMessage = "The asset tag cannot be more than 10 characters.")]
        [Display(Name = "Asset tag")]
        public string AssetTag { get; set; } = string.Empty;

        [Required(ErrorMessage = "The device name is required (e.g., HP ProBook 450).")]
        [StringLength(50, ErrorMessage = "The name cannot be more than 50 characters.")]
        [Display(Name = "Device name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The category is required (e.g., Laptop or Tablet).")]
        [StringLength(20, ErrorMessage = "The category cannot be more than 20 characters.")]
        [Display(Name = "Category")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "The purchase year is required.")]
        [Range(2015, 2026, ErrorMessage = "The year bought must be from 2015 to 2026.")]
        [Display(Name = "Year bought")]
        public int PurchaseYear { get; set; }

        [Display(Name = "On loan?")]
        public bool IsOnLoan { get; set; }
    }
}
