using System.ComponentModel.DataAnnotations;
namespace HR_Portal.Models
{
    public class RequestType
    {
        [Key]
        public int RequestTypeId { get; set; }

        [Required]
        public string? RequestName { get; set; }

        public string? AdditionalDetails { get; set; }

    }
}
