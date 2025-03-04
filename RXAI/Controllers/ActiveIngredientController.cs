using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RXAI.Context;
using RXAI.Entities;
using RXAI.Entities.RXAI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using static RXAI.Controllers.ActiveIngredientsController;

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
        }

        public class ActiveIngredientDto
        {
            public string DrugBankID { get; set; }
            public string IngredientName { get; set; }
            public List<ActiveIngredientVariantDto> Variants { get; set; }
        }


        public class ActiveIngredientVariantDto
        {
            public string Strength { get; set; }
            public string StrengthUnit { get; set; }
            public string DiseaseName { get; set; }
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
            public string ICDCode { get; set; }
            public string DiseaseName { get; set; }
        }

        public class ActiveIngredientCreateDTO
        {
            public string IngredientName { get; set; }
            public string Strength { get; set; }
            public string StrengthUnit { get; set; }
            public string ICDCode { get; set; }
        }

        public class UpdateIngredientDTO
        {
            public string Strength { get; set; }
            public string StrengthUnit { get; set; }
        }

        [HttpGet("bases")]
        public async Task<ActionResult<IEnumerable<ActiveIngredientBaseDTO>>> GetActiveIngredientBases()
        {
            return await _context.ActiveIngredientBases
                .Select(ai => new ActiveIngredientBaseDTO
                {
                    DrugBankID = ai.DrugBankID,
                    IngredientName = ai.IngredientName
                }).ToListAsync();
        }

        [HttpGet("bases/{drugBankID}")]
        public async Task<ActionResult<ActiveIngredientBaseDTO>> GetActiveIngredientBase(string drugBankID)
        {
            var ingredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.DrugBankID == drugBankID);

            if (ingredient == null) return NotFound("Active Ingredient Dosen't Exist");

            return new ActiveIngredientBaseDTO
            {
                DrugBankID = ingredient.DrugBankID,
                IngredientName = ingredient.IngredientName
            };
        }


        [HttpGet("bases/name/{ingredientName}")]
        public async Task<ActionResult<ActiveIngredientBaseDTO>> GetActiveIngredientBaseByName(string ingredientName)
        {
            var ingredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.IngredientName == ingredientName);

            if (ingredient == null) return NotFound("Active Ingredient Doesn't Exist");

            return new ActiveIngredientBaseDTO
            {
                DrugBankID = ingredient.DrugBankID,
                IngredientName = ingredient.IngredientName
            };
        }



        [HttpPost("bases")]
        public async Task<ActionResult<ActiveIngredientBaseDTO>> CreateActiveIngredientBase(ActiveIngredientBaseDTO dto)
        {
            var existingIngredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.DrugBankID == dto.DrugBankID);

            if (existingIngredient != null)
            {
                return BadRequest($"this id ({dto.DrugBankID})already exist.");
            }

            var ingredient = new ActiveIngredientBase
            {
                DrugBankID = dto.DrugBankID,
                IngredientName = dto.IngredientName
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

            if (ingredient == null) return NotFound("Active Ingredient Dosen't Exist");

            ingredient.IngredientName = dto.IngredientName;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("bases/{IngredientName}")]
        public async Task<IActionResult> DeleteActiveIngredientBase(string IngredientName)
        {
            var ingredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(ai => ai.IngredientName == IngredientName);

            if (ingredient == null) return NotFound("Active Ingredient Dosen't Exist");

            _context.ActiveIngredientBases.Remove(ingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("variants")]
        public async Task<ActionResult<IEnumerable<ActiveIngredientVariantDTO>>> GetActiveIngredientVariants()
        {
            return await _context.ActiveIngredientVariants
                .Include(v => v.BaseIngredient)
                .Include(v => v.Disease)
                .Select(v => new ActiveIngredientVariantDTO
                {
                    DrugBankID = v.DrugBankID,
                    IngredientName = v.BaseIngredient.IngredientName,
                    Strength = v.Strength,
                    StrengthUnit = v.StrengthUnit,
                    ICDCode = v.ICDCode,
                    DiseaseName = v.Disease.DiseaseName
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
                .Include(v => v.Disease)
                .FirstOrDefaultAsync(v =>
                    v.DrugBankID == baseIngredient.DrugBankID &&  
                    v.Strength == strength &&
                    v.StrengthUnit == strengthUnit);

            if (variant == null) return NotFound("Ingredient Variant Not Exist");

            return new ActiveIngredientVariantDTO
            {
                DrugBankID = variant.DrugBankID,
                IngredientName = baseIngredient.IngredientName,
                Strength = variant.Strength,
                StrengthUnit = variant.StrengthUnit,
                ICDCode = variant.ICDCode,
                DiseaseName = variant.Disease?.DiseaseName
            };
        }


        [HttpPost("variants")]
        public async Task<ActionResult<ActiveIngredientVariantDTO>> CreateActiveIngredientVariant(ActiveIngredientCreateDTO dto)
        {
            var baseIngredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(b => b.IngredientName == dto.IngredientName); // البحث بالاسم بدل الكود

            if (baseIngredient == null)
            {
                return BadRequest("Base Ingredient Not Exist");
            }

            if (!string.IsNullOrEmpty(dto.ICDCode))
            {
                var disease = await _context.Diseases
                    .FirstOrDefaultAsync(d => d.ICDCode == dto.ICDCode);

                if (disease == null)
                {
                    return BadRequest("ICD Code Not Exist");
                }
            }

            var duplicateVariant = await _context.ActiveIngredientVariants.AnyAsync(v =>
                v.DrugBankID == baseIngredient.DrugBankID && // استخدام `DrugBankID` المستخرج
                v.Strength == dto.Strength &&
                v.StrengthUnit == dto.StrengthUnit);

            if (duplicateVariant)
            {
                return BadRequest("Already Exist");
            }

            var variant = new ActiveIngredientVariant
            {
                DrugBankID = baseIngredient.DrugBankID, // استخراج الكود من الاسم
                Strength = dto.Strength,
                StrengthUnit = dto.StrengthUnit,
                ICDCode = dto.ICDCode
            };

            _context.ActiveIngredientVariants.Add(variant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetActiveIngredientVariant),
                new
                {
                    ingredientName = baseIngredient.IngredientName, // الإرجاع بالاسم
                    strength = variant.Strength,
                    strengthUnit = variant.StrengthUnit
                },
                new ActiveIngredientVariantDTO
                {
                    DrugBankID = variant.DrugBankID,
                    IngredientName = baseIngredient.IngredientName,
                    Strength = variant.Strength,
                    StrengthUnit = variant.StrengthUnit,
                    ICDCode = variant.ICDCode
                });
        }


        //    [HttpPut("variants/{drugBankID}/{strength}/{strengthUnit}")]
        //    public async Task<IActionResult> UpdateActiveIngredientVariant(
        //string drugBankID,
        //string strength,
        //string strengthUnit,
        //UpdateIngredientDTO dto)
        //    {
        //        var variant = await _context.ActiveIngredientVariants
        //            .FirstOrDefaultAsync(v =>
        //                v.DrugBankID == drugBankID &&
        //                v.Strength == strength &&
        //                v.StrengthUnit == strengthUnit);

        //        if (variant == null) return NotFound("mess");

        //        var duplicateVariant = await _context.ActiveIngredientVariants.AnyAsync(v =>
        //            v.DrugBankID == drugBankID &&
        //            v.Strength == dto.Strength &&
        //            v.StrengthUnit == dto.StrengthUnit);

        //        if (duplicateVariant)
        //        {
        //            return BadRequest("mess");
        //        }

        //        variant.Strength = dto.Strength;
        //        variant.StrengthUnit = dto.StrengthUnit;

        //        await _context.SaveChangesAsync();
        //        return NoContent();
        //    }

     

        [HttpGet("variants/by-disease/{icdCode}")]
        public async Task<ActionResult<IEnumerable<ActiveIngredientVariantDTO>>> GetVariantsByDisease(string icdCode)
        {
            var variants = await _context.ActiveIngredientVariants
                .Include(v => v.BaseIngredient)
                .Include(v => v.Disease)
                .Where(v => v.ICDCode == icdCode)
                .Select(v => new ActiveIngredientVariantDTO
                {
                    DrugBankID = v.DrugBankID,
                    IngredientName = v.BaseIngredient.IngredientName,
                    Strength = v.Strength,
                    StrengthUnit = v.StrengthUnit,
                    ICDCode = v.ICDCode,
                    DiseaseName = v.Disease.DiseaseName
                }).ToListAsync();

            if (!variants.Any()) return NotFound("There is no ICd COde ");

            return variants;
        }

        [HttpDelete("variants/{ingredientName}/{strength}/{strengthUnit}")]
        public async Task<IActionResult> DeleteActiveIngredientVariant(
                  string ingredientName,
                  string strength,
                  string strengthUnit)
        {
            var baseIngredient = await _context.ActiveIngredientBases
                .FirstOrDefaultAsync(b => b.IngredientName == ingredientName);

            if (baseIngredient == null) return NotFound("Ingredient Not Found");

            var variant = await _context.ActiveIngredientVariants
                .FirstOrDefaultAsync(v =>
                    v.DrugBankID == baseIngredient.DrugBankID &&
                    v.Strength == strength &&
                    v.StrengthUnit == strengthUnit);

            if (variant == null) return NotFound("Doesn't Exist");

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
                .Include(v => v.Disease)
                .Where(v => v.DrugBankID == baseIngredient.DrugBankID)
                .Select(v => new ActiveIngredientVariantDTO
                {
                    DrugBankID = v.DrugBankID,
                    IngredientName = v.BaseIngredient.IngredientName,
                    Strength = v.Strength,
                    StrengthUnit = v.StrengthUnit,
                    ICDCode = v.ICDCode,
                    DiseaseName = v.Disease.DiseaseName
                }).ToListAsync();

            if (!variants.Any()) return NotFound("No information Exists");

            return variants;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddActiveIngredientWithVariants([FromBody] ActiveIngredientDto dto)
        {
            try
            {
                if (dto == null || string.IsNullOrEmpty(dto.DrugBankID) || string.IsNullOrEmpty(dto.IngredientName))
                    return BadRequest(new { message = "Active ingredient data is incomplete." });

                // البحث عن المادة الفعالة في الداتا بيز
                var existingBase = await _context.ActiveIngredientBases
                    .Include(b => b.Variants)
                    .FirstOrDefaultAsync(b => b.DrugBankID == dto.DrugBankID);

                if (existingBase != null)
                {
                    // لو موجود، نتحقق إن الاسم متطابق مع اللي في قاعدة البيانات
                    if (existingBase.IngredientName != dto.IngredientName)
                    {
                        return BadRequest(new { message = "Conflict: DrugBankID is already associated with a different ingredient name." });
                    }
                }
                else
                {
                    // لو مش موجود، نضيف المادة الفعالة لأول مرة
                    existingBase = new ActiveIngredientBase
                    {
                        DrugBankID = dto.DrugBankID,
                        IngredientName = dto.IngredientName,
                        Variants = new List<ActiveIngredientVariant>()
                    };

                    _context.ActiveIngredientBases.Add(existingBase);
                }

                // معالجة الـ Variants
                foreach (var variantDto in dto.Variants)
                {
                    // التأكد من إدخال جميع البيانات
                    if (string.IsNullOrEmpty(variantDto.Strength) || string.IsNullOrEmpty(variantDto.StrengthUnit) || string.IsNullOrEmpty(variantDto.DiseaseName))
                    {
                        return BadRequest(new { message = "Each variant must have a strength, strength unit, and disease name." });
                    }

                    // البحث عن المرض باستخدام الاسم
                    var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.DiseaseName == variantDto.DiseaseName);
                    if (disease == null)
                    {
                        return BadRequest(new { message = $"Disease '{variantDto.DiseaseName}' not found." });
                    }

                    // التحقق من أن التركيز غير مكرر لنفس المادة الفعالة
                    bool variantExists = existingBase.Variants.Any(v =>
                        v.Strength == variantDto.Strength && v.StrengthUnit == variantDto.StrengthUnit);

                    if (!variantExists)
                    {
                        var newVariant = new ActiveIngredientVariant
                        {
                            DrugBankID = existingBase.DrugBankID,
                            Strength = variantDto.Strength,
                            StrengthUnit = variantDto.StrengthUnit,
                            ICDCode = disease.ICDCode // استخدام الكود المستخرج من اسم المرض
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

                // البحث عن المادة الفعالة
                var existingBase = await _context.ActiveIngredientBases
                    .Include(b => b.Variants)
                    .FirstOrDefaultAsync(b => b.IngredientName == dto.ActiveIngredientName);

                if (existingBase == null)
                {
                    return NotFound(new { message = "Active ingredient not found." });
                }

                // تحديث جميع الـ Variants المرتبطة بهذه المادة
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


        public class UpdateVariantDto
        {
            public string ActiveIngredientName { get; set; } // اسم المادة الفعالة
            public string NewStrength { get; set; } // التركيز الجديد
            public string NewStrengthUnit { get; set; } // وحدة التركيز الجديدة
        }


    }
}
    
