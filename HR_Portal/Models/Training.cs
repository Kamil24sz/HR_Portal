using System.ComponentModel.DataAnnotations;
namespace HR_Portal.Models
{
    public class Training
    {
        [Key]
        [Required]
        public int TrainingId { get; set; }

        [Required]
        public string? TrainingName { get; set; }

        public string? TrainingType { get; set; }

        public decimal Cost { get; set; }

        public string? Description { get; set; }
    }
}
