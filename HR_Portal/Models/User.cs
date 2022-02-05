using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HR_Portal.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }

        
        [Range(0, 50, ErrorMessage = "Manager ID is not properly.")]
        public int? ManagerId { get; set; }

        public User? Manager { get; set; }

        [Required]
        [Display(Name ="First Name")]
        [RegularExpression(@"^[A-Z]{1}[a-z]+$", ErrorMessage = "First Name should contains only letters, and start with capital letter.")]
        [StringLength(maximumLength: 15, MinimumLength = 2, ErrorMessage = "First name should contains characters between 2-15 characters.")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Z]{1}[a-z]+$", ErrorMessage = "Last Name should contains only letters, and start with capital letter.")]
        [StringLength(maximumLength: 15, MinimumLength = 2, ErrorMessage = "Last name should contains characters between 2-15 characters.")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd MM yyyy}")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 10, ErrorMessage = "Address should contains characters between 10-100 characters.")]
        public string? Address { get; set; }

        [Required]
        public string? Gender { get; set; }

        [Required]
        [Display(Name = "Document ID")]
        [RegularExpression(@"^[A-Z0-9]{8,20}$", ErrorMessage = "Document ID is not properly.")]
        public string? DocumentId { get; set; }

        
        [Range(0, 50, ErrorMessage = "Possition ID is not properly.")]
        public int? PositionId { get; set; }

        
        public Position? Position { get; set; }


        [Required]
        [Display(Name = "Vacation Days")]
        [Range(0,50, ErrorMessage ="Vacation days number is not in acceptable range.")]
        public int? VacationDays { get; set; }

        [Required]
        [Display(Name = "Bank Account")]
        [RegularExpression(@"^[A-Z0-9]{15,50}$", ErrorMessage = "Bank account should contain between 15-50 digits, and optionaly country prefix.")]
        public string? BankAccount { get; set; }

        [Required]
        [Display(Name = "Salary Bonus")]
        [Range(0,1000000,ErrorMessage = "Salary bonus not contains in range.")]
        public decimal? SalaryBonus { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

    }

}
