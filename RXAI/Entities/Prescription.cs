using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RXAI.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    namespace RXAI.Entities
    {
        public class Prescription
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int PrescriptionID { get; set; }
            public string Prescription_Description { get; set; }
            public string Dose { get; set; }
            public string Form { get; set; }
            public DateTime PrescriptionDate { get; set; }
            public string? Dispensedmedication { get; set; }

            [StringLength(20)]
            public string DrugBankID { get; set; }
            [StringLength(4)]
            public string Strength { get; set; }
            [StringLength(20)]
            public string StrengthUnit { get; set; }

            [ForeignKey("DrugBankID, Strength, StrengthUnit")]
            public virtual ActiveIngredientVariant ActiveIngredientVariant { get; set; }

            // حقل إضافي للمرجعية فقط
            [StringLength(100)]
            public string IngredientName { get; set; }

            public int QuantityDispensed { get; set; }
            public string PhoneNumber { get; set; }

            [ForeignKey("PhoneNumber")]
            public virtual Patient Patient { get; set; }
        }
    }
}
