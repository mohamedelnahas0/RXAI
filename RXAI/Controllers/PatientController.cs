using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RXAI.Context;
using RXAI.Dtos.PT;
using RXAI.Entities;

namespace RXAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {


        private readonly RXAIContext _context;

        public PatientController(RXAIContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients()
        {
            var patients = await _context.Patients
                .Select(p => new PatientDTO
                {
                    PhoneNumber = p.PhoneNumber,
                    PatientName = p.PatientName
                })
                .ToListAsync();

            return Ok(patients);
        }

        [HttpGet("{phoneNumber}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(string phoneNumber)
        {
            var patient = await _context.Patients
                .Where(p => p.PhoneNumber == phoneNumber)
                .Select(p => new PatientDTO
                {
                    PhoneNumber = p.PhoneNumber,
                    PatientName = p.PatientName
                })
                .FirstOrDefaultAsync();

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient([FromBody] PatientDTO patientDto)
        {
            if (await _context.Patients.AnyAsync(p => p.PhoneNumber == patientDto.PhoneNumber))
            {
                return BadRequest("Patient already exists.");
            }

            var patient = new Patient
            {
                PhoneNumber = patientDto.PhoneNumber,
                PatientName = patientDto.PatientName
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { phoneNumber = patient.PhoneNumber }, patientDto);
        }
        [HttpDelete("delete/{phoneNumber}")]
        public async Task<IActionResult> DeletePatient(string phoneNumber)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found." });
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Patient deleted successfully." });
        }

        [HttpPut("update/{phoneNumber}")]
        public async Task<IActionResult> UpdatePatient(string phoneNumber, updatepatientDto patientDto)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found." });
            }
            patient.PatientName = patientDto.PatientName;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Patient updated successfully." });


        }

        [HttpGet("patient-prescriptions/{phoneNumber}")]
        public async Task<IActionResult> GetPatientPrescriptions(string phoneNumber)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found." });
            }

            var prescriptions = await _context.Prescriptions
                .Where(p => p.PhoneNumber == phoneNumber)
                .Select(p => new
                {
                    p.Prescription_Description,
                    p.Dose,
                    p.Form,
                    p.PrescriptionDate,
                    p.DrugBankID,
                    p.Dispensedmedication,
                    p.QuantityDispensed
                })
                .ToListAsync();

            if (prescriptions.Count == 0)
            {
                return NotFound(new { Message = "No prescriptions found for this patient." });
            }

            return Ok(new
            {
                PatientName = patient.PatientName,
                PhoneNumber = patient.PhoneNumber,
                Prescriptions = prescriptions
            });
        }

    }

}

