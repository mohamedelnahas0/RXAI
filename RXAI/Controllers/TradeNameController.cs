using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RXAI.Context;
using RXAI.Dtos.TR;
using RXAI.Entities;

namespace RXAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeNameController : ControllerBase
    {
        private readonly RXAIContext _context;

        public TradeNameController(RXAIContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddTradeName([FromBody] TradeNameDto tradeNameDto)
        {
            var activeIngredientBase = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.IngredientName == tradeNameDto.IngredientName);

            if (activeIngredientBase == null)
                return NotFound("The active ingredient does not exist.");

            var activeIngredientVariant = await _context.ActiveIngredientVariants
                .Include(a => a.BaseIngredient)
                .FirstOrDefaultAsync(ai =>
                    ai.DrugBankID == activeIngredientBase.DrugBankID &&
                    ai.Strength == tradeNameDto.Strength &&
                    ai.StrengthUnit == tradeNameDto.StrengthUnit);

            if (activeIngredientVariant == null)
                return NotFound("The active ingredient in this strength and unit does not exist.");

            var exists = await _context.TradeNames
                .AnyAsync(t => t.SKUCode == tradeNameDto.Skucode && t.Name == tradeNameDto.Name);

            if (exists)
                return Conflict("This brand name with the same SKU code already exists.");

            var tradeName = new TradeName
            {
                SKUCode = tradeNameDto.Skucode,
                Name = tradeNameDto.Name,
                DrugBankID = activeIngredientBase.DrugBankID, // استخدام DrugBankID المستخرج
                PharmaceuticalForm = tradeNameDto.PharmaceuticalForm,
                Price = tradeNameDto.Price,
                QuantityStock = tradeNameDto.QuantityStock,
                ManufactureCountry = tradeNameDto.ManufactureCountry,
                IngredientName = activeIngredientBase.IngredientName,
                Strength = tradeNameDto.Strength,
                StrengthUnit = tradeNameDto.StrengthUnit
            };

            // إضافة التريد نيم إلى قاعدة البيانات
            _context.TradeNames.Add(tradeName);
            await _context.SaveChangesAsync();

            // إرجاع النتيجة
            return Ok(tradeName);
        }

        //[HttpGet("{Tradename}")]
        //public async Task<IActionResult> GetTradeName( string name)
        //{
        //    var tradeName = await _context.TradeNames.FirstOrDefaultAsync(t => t.Name == name);
        //    if (tradeName == null) return NotFound("Trade name not found.");
        //    return Ok(tradeName);
        //}


        [HttpGet("{atcCode}")]
        public async Task<IActionResult> GetTradeNamesByAtc(string atcCode)
        {
            var tradeNames = await _context.TradeNames
                .Where(t => t.SKUCode == atcCode)
                .ToListAsync();

            if (!tradeNames.Any()) return NotFound("No trade names found for this SKU Code.");

            return Ok(tradeNames);
        }


        [HttpGet("id/{drugBankId}")]
        public async Task<IActionResult> GetTradeNamesByDrugBankId(string drugBankId)
        {
            var tradeNames = await _context.TradeNames.Where(t => t.DrugBankID == drugBankId).ToListAsync();
            return Ok(tradeNames);
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchTradeNames(string name)
        {
            var tradeNames = await _context.TradeNames
                .Where(t => t.Name.Contains(name))
                .ToListAsync();

            if (!tradeNames.Any()) return NotFound("No matching trade names found.");

            return Ok(tradeNames);
        }



        [HttpGet("by-active/{activeIngredient}")]
        public async Task<IActionResult> GetTradeNamesByActiveIngredient(string activeIngredient)
        {
            var tradeNames = await _context.TradeNames
                .Where(t => t.ActiveIngredientVariant.BaseIngredient.IngredientName.Contains(activeIngredient))
                .ToListAsync();
            return Ok(tradeNames);
        }


        [HttpGet("by-activeDetails")]
        public async Task<IActionResult> GetTradeNamesByActiveIngredient(
    string activeIngredient, // اسم المادة الفعالة
    [FromQuery] string strength = null, // القوة (اختياري)
    [FromQuery] string strengthUnit = null) // وحدة القوة (اختياري)
        {
            // بناء الاستعلام الأساسي
            var query = _context.TradeNames
                .Where(t => t.ActiveIngredientVariant.BaseIngredient.IngredientName.Contains(activeIngredient));

            // إضافة شروط إضافية إذا تم تقديم Strength و StrengthUnit
            if (!string.IsNullOrEmpty(strength))
            {
                query = query.Where(t => t.ActiveIngredientVariant.Strength == strength);
            }

            if (!string.IsNullOrEmpty(strengthUnit))
            {
                query = query.Where(t => t.ActiveIngredientVariant.StrengthUnit == strengthUnit);
            }

            // تنفيذ الاستعلام وإرجاع النتيجة
            var tradeNames = await query.ToListAsync();

            if (!tradeNames.Any())
                return NotFound("No trade names found for the specified criteria.");

            return Ok(tradeNames);
        }
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateTradeName(string name, [FromBody] UpdateTradeDto tradeNameDto)
        {
            var tradeName = await _context.TradeNames.FirstOrDefaultAsync(t => t.Name == name);
            if (tradeName == null) return NotFound("Trade name not found.");

            //if (tradeNameDto.AtcCode != null && tradeNameDto.AtcCode != atcCode)
            //    return BadRequest("AtcCode cannot be modified.");

            //if (tradeNameDto.Name != null && tradeNameDto.Name != name)
            //    return BadRequest("Trade name cannot be modified.");
            tradeName.Name = tradeNameDto.Name;
            tradeName.PharmaceuticalForm = tradeNameDto.PharmaceuticalForm;
            tradeName.Price = tradeNameDto.Price;
            tradeName.QuantityStock = tradeNameDto.QuantityStock;
            tradeName.ManufactureCountry = tradeNameDto.ManufactureCountry;

            await _context.SaveChangesAsync();
            return Ok("Trade name updated successfully.");
        }



        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteTradeName(string name)
        {
            var tradeName = await _context.TradeNames.FirstOrDefaultAsync(t => t.Name == name);
            if (tradeName == null) return NotFound();

            _context.TradeNames.Remove(tradeName);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("by-sku/{skuCode}")]
        public async Task<IActionResult> GetTradeNameBySku(string skuCode)
        {
            // البحث عن التريد نيم باستخدام SKUCode
            var tradeName = await _context.TradeNames
                .FirstOrDefaultAsync(t => t.SKUCode == skuCode);

            if (tradeName == null)
                return NotFound("Trade name not found.");

            return Ok(tradeName);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetTradeNames(
    [FromQuery] string sortBy = "name", // القيمة الافتراضية: الفرز بالاسم
    [FromQuery] string sortOrder = "asc") // القيمة الافتراضية: تصاعدي (asc)
        {
            // جلب جميع الأدوية من قاعدة البيانات
            var query = _context.TradeNames.AsQueryable();

            // تطبيق الفرز حسب المعلمة المحددة
            switch (sortBy.ToLower())
            {
                case "price":
                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(t => t.Price)
                        : query.OrderBy(t => t.Price);
                    break;

                case "name":
                default:
                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(t => t.Name)
                        : query.OrderBy(t => t.Name);
                    break;
            }

            // تحويل النتيجة إلى DTO
            var tradeNames = await query
                .Select(t => new TradeNameListDto
                {
                    Name = t.Name,
                    Price = t.Price,
                    PharmaceuticalForm = t.PharmaceuticalForm,
                    ManufactureCountry = t.ManufactureCountry
                })
                .ToListAsync();

            return Ok(tradeNames);
        }
        public class TradeNameListDto
        {
            public string Name { get; set; } // اسم الدواء
            public decimal? Price { get; set; } // السعر
            public string PharmaceuticalForm { get; set; } // الفورم
            public string ManufactureCountry { get; set; } // دولة التصنيع
        }


    }

}
