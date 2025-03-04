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
            var activeIngredient = await _context.ActiveIngredientVariants
                .Include(a => a.BaseIngredient) 
                .FirstOrDefaultAsync(ai =>
                    ai.DrugBankID == tradeNameDto.DrugBankID &&
                    ai.Strength == tradeNameDto.Strength &&
                    ai.StrengthUnit == tradeNameDto.StrengtUnit);

            if (activeIngredient == null)
                return NotFound("The active ingredient in this strength and unity does not exist..");

            var exists = await _context.TradeNames
                .AnyAsync(t => t.SKUCode == tradeNameDto.Skucode && t.Name == tradeNameDto.Name);

            if (exists)
                return Conflict("This brand name with the same ATC code already exists..");

            var tradeName = new TradeName
            {
                SKUCode = tradeNameDto.Skucode,
                Name = tradeNameDto.Name,
                DrugBankID = tradeNameDto.DrugBankID,
                PharmaceuticalForm = tradeNameDto.PharmaceuticalForm,
                Price = tradeNameDto.Price,
                QuantityStock = tradeNameDto.QuantityStock,
                ManufactureCountry = tradeNameDto.ManufactureCountry,
                IngredientName = activeIngredient.BaseIngredient.IngredientName,
                Strength = tradeNameDto.Strength,
                StrengthUnit = tradeNameDto.StrengtUnit
            };

            _context.TradeNames.Add(tradeName);
            await _context.SaveChangesAsync();

            return Ok(tradeName);
        }

        [HttpGet("{atcCode}/{name}")]
        public async Task<IActionResult> GetTradeName(string atcCode, string name)
        {
            var tradeName = await _context.TradeNames.FirstOrDefaultAsync(t => t.SKUCode == atcCode && t.Name == name);
            if (tradeName == null) return NotFound("Trade name not found.");
            return Ok(tradeName);
        }


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
                .Where(t => t.ActiveIngredientVariant.DrugBankID.Contains(activeIngredient))
                .ToListAsync();
            return Ok(tradeNames);
        }

        [HttpPut("{atcCode}/{name}")]
        public async Task<IActionResult> UpdateTradeName(string atcCode, string name, [FromBody] UpdateTradeDto tradeNameDto)
        {
            var tradeName = await _context.TradeNames.FirstOrDefaultAsync(t => t.SKUCode == atcCode && t.Name == name);
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



        [HttpDelete("{atcCode}/{name}")]
        public async Task<IActionResult> DeleteTradeName(string atcCode, string name)
        {
            var tradeName = await _context.TradeNames.FirstOrDefaultAsync(t => t.SKUCode == atcCode && t.Name == name);
            if (tradeName == null) return NotFound();

            _context.TradeNames.Remove(tradeName);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }

}
