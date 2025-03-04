using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RXAI.Entities.RXAI.Entities;
using System.Text.Json.Serialization;

namespace RXAI.Entities
{
    public class ActiveIngredientVariant
    {
        [Key, ForeignKey("BaseIngredient")]
        [Column(Order = 0)]
        [StringLength(20)]
        public string DrugBankID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string Strength { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string StrengthUnit { get; set; }
        [JsonIgnore]
        public virtual ActiveIngredientBase BaseIngredient { get; set; }


   
        [JsonIgnore]
        public virtual ICollection<TradeName> Trades { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}