using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RXAI.Context;
using RXAI.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RXAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveIngredientsController : ControllerBase
    {
        private readonly RXAIContext _context;

        public ActiveIngredientsController(RXAIContext context)
        {
            _context = context;
        }

        public class ActiveIngredientBaseDTO
        {
            public string DrugBankID { get; set; }
            public string IngredientName { get; set; }
            public string DiseaseName { get; set; } // Added ICDCode
        }

        public class ActiveIngredientDto
        {
            public string DrugBankID { get; set; }
            public string IngredientName { get; set; }
            public string DiseaseName { get; set; }
            public List<ActiveIngredientVariantDto> Variants { get; set; }
        }

        public class ActiveIngredientVariantDto
        {
            public string Strength { get; set; }
            public string StrengthUnit { get; set; }
        }

        public class UpdateActiveIngredientBaseDTO
        {
            public string IngredientName { get; set; }
        }

        public class ActiveIngredientVariantDTO
        {
            public string DrugBankID { get; set; }
            public string Strength { get; set; }
            public string StrengthUnit { get; set; }
            public string IngredientName { get; set; }
        }

        public class ActiveIngredientCreateDTO
        {
            public string IngredientName { get; set; }
            public string Strength { get; set; }
            public string StrengthUnit { get; set; }
        }

        public class UpdateIngredientDTO
        {
            public string Strength { get; set; }
            public string StrengthUnit { get; set; }
        }

        public class UpdateVariantDto
        {

            public string ActiveIngredientName { get; set; }
            public string NewStrength { get; set; }
            public string NewStrengthUnit { get; set; }
        }
        public class UpdateActiveIngredientDTO
        {
            public string ActiveIngredientName { get; set; }
    
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateActiveIngredient(string id, [FromBody] UpdateActiveIngredientDTO dto)
        {
            var ingredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.DrugBankID == id);

            if (ingredient == null) return NotFound("Active Ingredient Not Found");

            ingredient.IngredientName = dto.ActiveIngredientName;

            await _context.SaveChangesAsync();

            return Ok("Active Ingredient Updated Successfully");
        }


        [HttpGet("bases")]
        public async Task<ActionResult<IEnumerable<ActiveIngredientBaseDTO>>> GetActiveIngredientBases()
        {
            return await _context.ActiveIngredientBases
                .Include(ai => ai.Disease) 
                .Select(ai => new ActiveIngredientBaseDTO
                {
                    DrugBankID = ai.DrugBankID,
                    IngredientName = ai.IngredientName,
                    DiseaseName = ai.Disease.DiseaseName 
                }).ToListAsync();
        }

        [HttpGet("bases/{drugBankID}")]
        public async Task<ActionResult<ActiveIngredientBaseDTO>> GetActiveIngredientBase(string drugBankID)
        {
            var ingredient = await _context.ActiveIngredientBases
                .Include(ai => ai.Disease) 
                .FirstOrDefaultAsync(ai => ai.DrugBankID == drugBankID);

            if (ingredient == null) return NotFound("Active Ingredient Doesn't Exist");

            return new ActiveIngredientBaseDTO
            {
                DrugBankID = ingredient.DrugBankID,
                IngredientName = ingredient.IngredientName,
                DiseaseName = ingredient.Disease.DiseaseName
            };
        }

        [HttpGet("bases/name/{ingredientName}")]
        public async Task<ActionResult<ActiveIngredientBaseDTO>> GetActiveIngredientBaseByName(string ingredientName)
        {
            var ingredient = await _context.ActiveIngredientBases
                .Include(ai => ai.Disease)
                .FirstOrDefaultAsync(ai => ai.IngredientName == ingredientName);

            if (ingredient == null) return NotFound("Active Ingredient Doesn't Exist");

            return new ActiveIngredientBaseDTO
            {
                DrugBankID = ingredient.DrugBankID,
                IngredientName = ingredient.IngredientName,
                DiseaseName = ingredient.Disease.DiseaseName 
            };
        }

        [HttpPost("bases")]
        public async Task<ActionResult<ActiveIngredientBaseDTO>> CreateActiveIngredientBase(ActiveIngredientBaseDTO dto)
        {
            var existingIngredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.DrugBankID == dto.DrugBankID);

            if (existingIngredient != null)
            {
                return BadRequest($"This ID ({dto.DrugBankID}) already exists.");
            }

            var disease = await _context.Diseases
                .FirstOrDefaultAsync(d => d.DiseaseName == dto.DiseaseName);

            if (disease == null)
            {
                return BadRequest($"Disease with name '{dto.DiseaseName}' not found.");
            }

            var ingredient = new ActiveIngredientBase
            {
                DrugBankID = dto.DrugBankID,
                IngredientName = dto.IngredientName,
                ICDCode = disease.ICDCode
            };

            _context.ActiveIngredientBases.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActiveIngredientBase), new { drugBankID = ingredient.DrugBankID }, dto);
        }

        [HttpPut("bases/{IngredientName}")]
        public async Task<IActionResult> UpdateActiveIngredientBase(string IngredientName, UpdateActiveIngredientBaseDTO dto)
        {
            var ingredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.IngredientName == IngredientName);

            if (ingredient == null) return NotFound("Active Ingredient Doesn't Exist");


            ingredient.IngredientName = dto.IngredientName;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("bases/{IngredientName}")]
        public async Task<IActionResult> DeleteActiveIngredientBase(string IngredientName)
        {
            var ingredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.IngredientName == IngredientName);

            if (ingredient == null) return NotFound("Active Ingredient Doesn't Exist");

            _context.ActiveIngredientBases.Remove(ingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("bases/id/{drugBankID}")]
        public async Task<IActionResult> DeleteActiveIngredientBaseByDrugBankID(string drugBankID)
        {
            var ingredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.DrugBankID == drugBankID);

            if (ingredient == null) return NotFound("Active Ingredient Doesn't Exist");

            _context.ActiveIngredientBases.Remove(ingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("variants")]
        public async Task<ActionResult<IEnumerable<ActiveIngredientVariantDTO>>> GetActiveIngredientVariants()
        {
            return await _context.ActiveIngredientVariants
                .Include(v => v.BaseIngredient)
                .Select(v => new ActiveIngredientVariantDTO
                {
                    DrugBankID = v.DrugBankID,
                    IngredientName = v.BaseIngredient.IngredientName,
                    Strength = v.Strength,
                    StrengthUnit = v.StrengthUnit
                }).ToListAsync();
        }
        [HttpGet("variants/{ingredientName}/{strength}/{strengthUnit}")]
        public async Task<ActionResult<ActiveIngredientVariantDTO>> GetActiveIngredientVariant(string ingredientName, string strength, string strengthUnit)
        {
            var baseIngredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(b => b.IngredientName == ingredientName);

            if (baseIngredient == null) return NotFound("Ingredient Not Found");

            var variant = await _context.ActiveIngredientVariants
                .Include(v => v.BaseIngredient)
                .FirstOrDefaultAsync(v =>
                    v.DrugBankID == baseIngredient.DrugBankID &&
                    v.Strength == strength &&
                    v.StrengthUnit == strengthUnit);

            if (variant == null) return NotFound("Ingredient Variant Doesn't Exist");

            return new ActiveIngredientVariantDTO
            {
                DrugBankID = variant.DrugBankID,
                IngredientName = baseIngredient.IngredientName,
                Strength = variant.Strength,
                StrengthUnit = variant.StrengthUnit
            };
        }
        [HttpPost("variants")]
        public async Task<ActionResult<ActiveIngredientVariantDTO>> CreateActiveIngredientVariant(ActiveIngredientCreateDTO dto)
        {
            var baseIngredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(b => b.IngredientName == dto.IngredientName);

            if (baseIngredient == null)
            {
                return BadRequest("Base Ingredient Doesn't Exist");
            }

            var duplicateVariant = await _context.ActiveIngredientVariants.AnyAsync(v =>
                v.DrugBankID == baseIngredient.DrugBankID &&
                v.Strength == dto.Strength &&
                v.StrengthUnit == dto.StrengthUnit);

            if (duplicateVariant)
            {
                return BadRequest("Variant Already Exists");
            }

            var variant = new ActiveIngredientVariant
            {
                DrugBankID = baseIngredient.DrugBankID, // الربط بالمادة الفعالة
                Strength = dto.Strength,
                StrengthUnit = dto.StrengthUnit
            };

            _context.ActiveIngredientVariants.Add(variant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetActiveIngredientVariant),
                new
                {
                    ingredientName = baseIngredient.IngredientName,
                    strength = variant.Strength,
                    strengthUnit = variant.StrengthUnit
                },
                new ActiveIngredientVariantDTO
                {
                    DrugBankID = variant.DrugBankID,
                    IngredientName = baseIngredient.IngredientName,
                    Strength = variant.Strength,
                    StrengthUnit = variant.StrengthUnit
                });
        }

        [HttpGet("variants/by-disease/{icdCode}")]
        public async Task<ActionResult<IEnumerable<ActiveIngredientVariantDTO>>> GetVariantsByDisease(string icdCode)
        {
            var variants = await _context.ActiveIngredientBases
                .Where(b => b.ICDCode == icdCode)
                .SelectMany(b => b.Variants)
                .Include(v => v.BaseIngredient)
                .Select(v => new ActiveIngredientVariantDTO
                {
                    DrugBankID = v.DrugBankID,
                    IngredientName = v.BaseIngredient.IngredientName,
                    Strength = v.Strength,
                    StrengthUnit = v.StrengthUnit
                }).ToListAsync();

            if (!variants.Any()) return NotFound("No Variants Found for This Disease");

            return variants;
        }
        [HttpDelete("variants/{ingredientName}/{strength}/{strengthUnit}")]
        public async Task<IActionResult> DeleteActiveIngredientVariant(string ingredientName, string strength, string strengthUnit)
        {
            var baseIngredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(b => b.IngredientName == ingredientName);

            if (baseIngredient == null) return NotFound("Ingredient Not Found");

            var variant = await _context.ActiveIngredientVariants
                .FirstOrDefaultAsync(v =>
                    v.DrugBankID == baseIngredient.DrugBankID &&
                    v.Strength == strength &&
                    v.StrengthUnit == strengthUnit);

            if (variant == null) return NotFound("Variant Doesn't Exist");

            _context.ActiveIngredientVariants.Remove(variant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("variants/by-base/{ingredientName}")]
        public async Task<ActionResult<IEnumerable<ActiveIngredientVariantDTO>>> GetVariantsByBase(string ingredientName)
        {
            var baseIngredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(b => b.IngredientName == ingredientName);

            if (baseIngredient == null) return NotFound("Ingredient Not Found");

            var variants = await _context.ActiveIngredientVariants
                .Include(v => v.BaseIngredient)
                .Where(v => v.DrugBankID == baseIngredient.DrugBankID)
                .Select(v => new ActiveIngredientVariantDTO
                {
                    DrugBankID = v.DrugBankID,
                    IngredientName = v.BaseIngredient.IngredientName,
                    Strength = v.Strength,
                    StrengthUnit = v.StrengthUnit
                }).ToListAsync();

            if (!variants.Any()) return NotFound("No Variants Found");

            return variants;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddActiveIngredientWithVariants([FromBody] ActiveIngredientDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.DrugBankID) || string.IsNullOrEmpty(dto.IngredientName))
                    return BadRequest(new { message = "Active ingredient data is incomplete." });

                var disease = await _context.Diseases
                    .FirstOrDefaultAsync(d => d.DiseaseName == dto.DiseaseName);

                if (disease == null)
                {
                    return BadRequest(new { message = $"Disease with name '{dto.DiseaseName}' not found." });
                }

                var existingBase = await _context.ActiveIngredientBases
                    .Include(b => b.Variants)
                    .FirstOrDefaultAsync(b => b.DrugBankID == dto.DrugBankID);

                if (existingBase != null)
                {
                    if (existingBase.IngredientName != dto.IngredientName)
                    {
                        return BadRequest(new { message = "Conflict: DrugBankID is already associated with a different ingredient name." });
                    }
                }
                else
                {
                    existingBase = new ActiveIngredientBase
                    {
                        DrugBankID = dto.DrugBankID,
                        IngredientName = dto.IngredientName,
                        ICDCode = disease.ICDCode, 
                        Variants = new List<ActiveIngredientVariant>()
                    };

                    _context.ActiveIngredientBases.Add(existingBase);
                }

                foreach (var variantDto in dto.Variants)
                {
                    if (string.IsNullOrEmpty(variantDto.Strength) || string.IsNullOrEmpty(variantDto.StrengthUnit))
                    {
                        return BadRequest(new { message = "Each variant must have a strength and strength unit." });
                    }

                    bool variantExists = existingBase.Variants.Any(v =>
                        v.Strength == variantDto.Strength && v.StrengthUnit == variantDto.StrengthUnit);

                    if (!variantExists)
                    {
                        var newVariant = new ActiveIngredientVariant
                        {
                            DrugBankID = existingBase.DrugBankID,
                            Strength = variantDto.Strength,
                            StrengthUnit = variantDto.StrengthUnit
                        };

                        existingBase.Variants.Add(newVariant);
                    }
                    else
                    {
                        return BadRequest(new { message = $"Variant with Strength '{variantDto.Strength} {variantDto.StrengthUnit}' already exists for this active ingredient." });
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Active ingredient and variants added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateVariant([FromBody] UpdateVariantDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.ActiveIngredientName) ||
                    string.IsNullOrEmpty(dto.NewStrength) || string.IsNullOrEmpty(dto.NewStrengthUnit))
                {
                    return BadRequest(new { message = "Incomplete update data." });
                }

                var existingBase = await _context.ActiveIngredientBases
                    .Include(b => b.Variants)
                    .FirstOrDefaultAsync(b => b.IngredientName == dto.ActiveIngredientName);

                if (existingBase == null)
                {
                    return NotFound(new { message = "Active ingredient not found." });
                }

                foreach (var variant in existingBase.Variants)
                {
                    variant.Strength = dto.NewStrength;
                    variant.StrengthUnit = dto.NewStrengthUnit;
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Variants updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the variant.", error = ex.Message });
            }
        }
    }
}