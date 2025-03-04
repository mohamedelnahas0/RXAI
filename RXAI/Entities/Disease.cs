using System.ComponentModel.DataAnnotations;

namespace RXAI.Entities
{
    public class Disease
    {
        [Key]
        [StringLength(20)]
        public string ICDCode { get; set; }

        [Required]
        [StringLength(100)]
        public string DiseaseName { get; set; }

        public virtual ICollection<ActiveIngredientVariant> ActiveIngredientVariants { get; set; }
    }
}
