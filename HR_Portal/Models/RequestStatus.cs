using System.ComponentModel.DataAnnotations;
namespace HR_Portal.Models
{
    public class RequestStatus
    {
        [Key]
        public int StatusId { get; set; }

        public string StatusName { get; set; }

    }
}
