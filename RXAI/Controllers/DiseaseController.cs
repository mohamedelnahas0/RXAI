using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RXAI.Context;
using RXAI.Dtos;
using RXAI.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RXAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseController : ControllerBase
    {
        private readonly RXAIContext _context;

        public DiseaseController(RXAIContext context)
        {
            _context = context;
        }

        // DTOs
        public class DiseaseDto
        {
            public string ICDCode { get; set; }
            public string DiseaseName { get; set; }
        }

        public class UpdateDiseaseDto
        {
            public string DiseaseName { get; set; }
        }

        // Methods

        /// <summary>
        /// Retrieve all diseases.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseases()
        {
            return await _context.Diseases
                .Select(d => new DiseaseDto { ICDCode = d.ICDCode, DiseaseName = d.DiseaseName })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieve a specific disease by ICDCode.
        /// </summary>
        /// <param name="id">The ICDCode of the disease.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<DiseaseDto>> GetDisease(string id)
        {
            var disease = await _context.Diseases.FindAsync(id);
            if (disease == null) return NotFound("Disease not found.");
            return new DiseaseDto { ICDCode = disease.ICDCode, DiseaseName = disease.DiseaseName };
        }

        /// <summary>
        /// Retrieve a specific disease by name.
        /// </summary>
        /// <param name="name">The name of the disease.</param>
        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<DiseaseDto>> GetDiseaseByName(string name)
        {
            var disease = await _context.Diseases
                .FirstOrDefaultAsync(d => d.DiseaseName == name);

            if (disease == null) return NotFound("Disease not found.");
            return new DiseaseDto { ICDCode = disease.ICDCode, DiseaseName = disease.DiseaseName };
        }

        /// <summary>
        /// Create a new disease.
        /// </summary>
        /// <param name="dto">The disease data.</param>
        [HttpPost]
        public async Task<ActionResult<DiseaseDto>> CreateDisease(DiseaseDto dto)
        {
            // Check for duplicate ICDCode or DiseaseName
            if (await _context.Diseases.AnyAsync(d => d.ICDCode == dto.ICDCode || d.DiseaseName == dto.DiseaseName))
                return BadRequest("Duplicate entry: ICDCode or DiseaseName already exists.");

            var disease = new Disease { ICDCode = dto.ICDCode, DiseaseName = dto.DiseaseName };
            _context.Diseases.Add(disease);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDisease), new { id = disease.ICDCode }, dto);
        }

        /// <summary>
        /// Update an existing disease.
        /// </summary>
        /// <param name="id">The ICDCode of the disease to update.</param>
        /// <param name="dto">The updated disease data.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDisease(string id, UpdateDiseaseDto dto)
        {
            var disease = await _context.Diseases.FindAsync(id);
            if (disease == null) return NotFound("Disease not found.");

            // Check if the new DiseaseName already exists
            if (await _context.Diseases.AnyAsync(d => d.DiseaseName == dto.DiseaseName && d.ICDCode != id))
                return BadRequest("Duplicate entry: DiseaseName already exists.");

            disease.DiseaseName = dto.DiseaseName;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a disease.
        /// </summary>
        /// <param name="id">The ICDCode of the disease to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisease(string id)
        {
            var disease = await _context.Diseases.FindAsync(id);
            if (disease == null) return NotFound("Disease not found.");

            _context.Diseases.Remove(disease);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retrieve diseases associated with a specific active ingredient by name.
        /// </summary>
        /// <param name="ingredientName">The name of the active ingredient.</param>
        [HttpGet("by-active-ingredient/{ingredientName}")]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseasesByActiveIngredient(string ingredientName)
        {
            var diseases = await _context.ActiveIngredientBases
                .Where(ai => ai.IngredientName == ingredientName)
                .Select(ai => ai.Disease)
                .Distinct()
                .Select(d => new DiseaseDto { ICDCode = d.ICDCode, DiseaseName = d.DiseaseName })
                .ToListAsync();

            if (!diseases.Any()) return NotFound("No diseases found for this active ingredient.");
            return diseases;
        }

        /// <summary>
        /// Retrieve diseases associated with a specific active ingredient by DrugBankID.
        /// </summary>
        /// <param name="drugBankId">The DrugBankID of the active ingredient.</param>
        [HttpGet("by-active-ingredient-id/{drugBankId}")]
        public async Task<ActionResult<IEnumerable<DiseaseDto>>> GetDiseasesByActiveIngredientId(string drugBankId)
        {
            var diseases = await _context.ActiveIngredientBases
                .Where(ai => ai.DrugBankID == drugBankId)
                .Select(ai => ai.Disease)
                .Distinct()
                .Select(d => new DiseaseDto { ICDCode = d.ICDCode, DiseaseName = d.DiseaseName })
                .ToListAsync();

            if (!diseases.Any()) return NotFound("No diseases found for this active ingredient.");
            return diseases;
        }
    }
}