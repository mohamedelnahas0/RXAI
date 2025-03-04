using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace RXAI.Entities
{
    public class TradeName
    {
        [Key]
        [StringLength(20)]
        public string SKUCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // المفتاح الخارجي للمتغير المحدد
        [StringLength(20)]
        public string DrugBankID { get; set; }
        [StringLength(4)]
        public string Strength { get; set; }
        [StringLength(20)]
        public string StrengthUnit { get; set; }

        // علاقة المفتاح الخارجي مع جدول متغيرات المكون الفعال
        [ForeignKey("DrugBankID, Strength, StrengthUnit")]
        public virtual ActiveIngredientVariant ActiveIngredientVariant { get; set; }

        // حقل إضافي للمرجعية فقط
        [StringLength(100)]
        public string IngredientName { get; set; }

        [StringLength(50)]
        public string PharmaceuticalForm { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityStock { get; set; }
        [StringLength(50)]
        public string ManufactureCountry { get; set; }
    }
}