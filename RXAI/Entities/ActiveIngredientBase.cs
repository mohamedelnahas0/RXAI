using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RXAI.Entities
{
    public class ActiveIngredientBase
    {
        [Key]
        [StringLength(20)]
        public string DrugBankID { get; set; }

        [Required]
        [StringLength(100)]
        public string IngredientName { get; set; }

        [StringLength(20)]
        public string ICDCode { get; set; }

        [ForeignKey("ICDCode")]
        public virtual Disease Disease { get; set; }
        public virtual ICollection<ActiveIngredientVariant> Variants { get; set; }
    }
    }
    
