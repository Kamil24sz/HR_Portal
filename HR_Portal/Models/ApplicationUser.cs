using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HR_Portal.Models
{
    public class ApplicationUser : IdentityUser
    {

        public int? UserId { get; set; }

        public User? User { get; set; }

    }
}
