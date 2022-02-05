using System.ComponentModel.DataAnnotations;
namespace HR_Portal.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        public int RequestTypeId { get; set; }

        public RequestType? RequestType { get; set; }

        public int RequestorId { get; set; }

        public User? Requestor { get; set; }

        public int StatusId { get; set; }

        public RequestStatus? Status { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

    }
}
