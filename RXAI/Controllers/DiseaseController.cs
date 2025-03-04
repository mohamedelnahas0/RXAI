//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using RXAI.Context;
//using RXAI.Dtos;
//using RXAI.Dtos.AI;
//using RXAI.Entities;

//namespace RXAI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DiseaseController : ControllerBase
//    {
//        private readonly RXAIContext _context;

//        public DiseaseController(RXAIContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseases()
//        {
//            return await _context.Diseases
//                .Select(d => new DiseaseDto { ICDCode = d.ICDCode, DiseaseName = d.DiseaseName })
//                .ToListAsync();
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<DiseaseDto>> GetDisease(string id)
//        {
//            var disease = await _context.Diseases.FindAsync(id);
//            if (disease == null) return NotFound();
//            return new DiseaseDto { ICDCode = disease.ICDCode, DiseaseName = disease.DiseaseName };
//        }


//        [HttpGet("by-name/{name}")]
//        public async Task<ActionResult<DiseaseDto>> GetDiseaseByName(string name)
//        {
//            var disease = await _context.Diseases.FirstOrDefaultAsync(d => d.DiseaseName == name);
//            if (disease == null) return NotFound();
//            return new DiseaseDto { ICDCode = disease.ICDCode, DiseaseName = disease.DiseaseName };
//        }

//        [HttpPost]
//        public async Task<ActionResult<DiseaseDto>> CreateDisease(DiseaseDto dto)
//        {
//            if (await _context.Diseases.AnyAsync(d => d.ICDCode == dto.ICDCode || d.DiseaseName == dto.DiseaseName))
//                return BadRequest("Duplicate entry.");

//            var disease = new Disease { ICDCode = dto.ICDCode, DiseaseName = dto.DiseaseName };
//            _context.Diseases.Add(disease);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetDisease), new { id = disease.ICDCode }, dto);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateDisease(string id, UpdateDiseaseDto dto)
//        {


//            var disease = await _context.Diseases.FindAsync(id);
//            if (disease == null) return NotFound();

//            disease.DiseaseName = dto.DiseaseName;
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteDisease(string id)
//        {
//            var disease = await _context.Diseases.FindAsync(id);
//            if (disease == null) return NotFound();

//            _context.Diseases.Remove(disease);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpGet("by-active-ingredient/{ingredientName}")]
//        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseasesByActiveIngredient(string ingredientName)
//        {
//            var diseases = await _context.ActiveIngredients
//                .Where(ai => ai.IngredientName == ingredientName)
//                .Select(ai => ai.Disease)
//                .Distinct()
//                .Select(d => new DiseaseDto { ICDCode = d.ICDCode, DiseaseName = d.DiseaseName })
//                .ToListAsync();

//            if (!diseases.Any()) return NotFound();
//            return diseases;
//        }
//        [HttpGet("by-active-ingredient-id/{drugBankId}")]
//        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseasesByActiveIngredientId(string drugBankId)
//        {
//            var diseases = await _context.ActiveIngredients
//                .Where(ai => ai.DrugBankID == drugBankId)
//                .Select(ai => ai.Disease)
//                .Distinct()
//                .Select(d => new DiseaseDto { ICDCode = d.ICDCode, DiseaseName = d.DiseaseName })
//                .ToListAsync();

//            if (!diseases.Any()) return NotFound();
//            return diseases;
//        }
//    }
//}

