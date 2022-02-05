using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HR_Portal.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        public string? DepartmentName { get; set; }

        public int? DirectorId { get; set; }

        public User? Director { get; set; }

    }
}
