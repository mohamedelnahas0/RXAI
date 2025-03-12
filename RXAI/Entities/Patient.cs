using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RXAI.Entities.RXAI.Entities;

namespace RXAI.Entities
{
    public class Patient
    {
        [Key]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string PatientName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
