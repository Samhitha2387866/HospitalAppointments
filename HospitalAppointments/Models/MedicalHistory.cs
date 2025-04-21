using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NewHospitalManagementSystem.Models
{
    public class MedicalHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("PatientRegistration")]
        public int PatientId { get; set; }
        [Required]
        public string PatientName { get; set; }
        [Required]
        [ForeignKey("DoctorRegistration")]
        public int DoctorId { get; set; }
        [Required]
        public DateOnly VisitDate { get; set; }
        [Required]
        [StringLength(500)]
        public string Treatment { get; set; }
        [StringLength(500)]
        public string Medicines_prescribed { get; set; }
    }
}

