using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NewHospitalManagementSystem.Models
{
    public class PatientRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient Name is required.")]
        [StringLength(100, ErrorMessage = "Patient Name cannot exceed 100 characters.")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = "Contact Number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact Number must be exactly 10 digits.")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Emergency Contact is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Emergency Contact must be exactly 10 digits.")]
        public string EmergencyContact { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}

