using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.AppService.DTO
{
    public class CreateTenantDto
    {
        [Required(ErrorMessage = "Company name cannot be null or empty")]
        [StringLength(50, ErrorMessage = "Company name cannot exceed 50 characters in length")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "First name cannot be null or empty")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters in length")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name cannot be null or empty")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters in length")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email cannot be null or empty")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters in length")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be null or empty")]
        [MinLength(8, ErrorMessage = "Password cannot be less than 8 characters in length")]
        [StringLength(20, ErrorMessage = "Password cannot exceed 20 characters in length")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)",
        ErrorMessage = "Password must include at least one uppercase letter, one lowercase letter, and one digit.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
