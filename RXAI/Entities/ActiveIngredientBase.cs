using System.ComponentModel.DataAnnotations;

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

        public virtual ICollection<ActiveIngredientVariant> Variants { get; set; }
    }
    }
    
