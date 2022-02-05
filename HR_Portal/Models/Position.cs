using System.ComponentModel.DataAnnotations;


namespace HR_Portal.Models
{
    public class Position
    {
        [Key]
        public int? PositionId { get; set; }

        [Required]
        public string? PositionName { get; set; }

        public int? DepartmentId { get; set; }
        
        public Department? Department { get; set; }

        [Required]
        [Display(Name = "Base Salary")]
        [Range(0, 1000000, ErrorMessage = "Base Salary not contains in range.")]
        public decimal? BaseSalary { get; set; }

    }
}
