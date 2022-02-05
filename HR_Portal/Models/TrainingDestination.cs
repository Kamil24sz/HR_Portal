using System.ComponentModel.DataAnnotations;
namespace HR_Portal.Models
{
    public class TrainingDestination
    {
        [Key]
        public int DestinationId { get; set; }

        public int TrainingId { get; set; }

        public Training? Training { get; set; }

        public int UserId { get; set; }


        public User? User { get; set; }


        public int? Progress { get; set; }

    }
}
