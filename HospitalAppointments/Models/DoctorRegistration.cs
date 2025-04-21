using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NewHospitalManagementSystem.Models
{
    public class DoctorRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorId { get; set; }
        [Required(ErrorMessage = "Doctor Name is required.")]
        [StringLength(100, ErrorMessage = "Doctor Name cannot exceed 100 characters.")]
        public string DoctorName { get; set; }
        [Required(ErrorMessage = "Specialization is required.")]
        [StringLength(100, ErrorMessage = "Specialization cannot exceed 100 characters.")]
        public string Specialization { get; set; }
        [Required(ErrorMessage = "Contact Number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact Number must be exactly 10 characters.")]
        [Phone(ErrorMessage = "Contact Number is not valid.")]
        public string ContactNumber { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(10, ErrorMessage = "Gender cannot exceed 10 characters.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
