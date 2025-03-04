using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RXAI.Context;
using RXAI.Entities;
using RXAI.Entities.RXAI.Entities;

namespace RXAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly RXAIContext _context;

        public PrescriptionController(RXAIContext context)
        {
            _context = context;
        }

        [HttpPost("add-and-dispense")]
        public async Task<IActionResult> AddAndDispense(
      [FromQuery] string IngredientName,
      [FromQuery] string dose,
      [FromQuery] string form,
      [FromQuery] string strength,
      [FromQuery] string strengthUnit,
      [FromQuery] string phoneNumber,
      [FromQuery] string prescriptionDescription,
      [FromQuery] string dispensedMedication,
      [FromQuery] int quantity)
        {
            if (string.IsNullOrEmpty(IngredientName) || string.IsNullOrEmpty(dose) ||
                string.IsNullOrEmpty(form) || string.IsNullOrEmpty(strength) ||
                string.IsNullOrEmpty(strengthUnit) || string.IsNullOrEmpty(phoneNumber) ||
                string.IsNullOrEmpty(prescriptionDescription) || string.IsNullOrEmpty(dispensedMedication) ||
                quantity <= 0)
            {
                return BadRequest(new { Message = "All fields and a valid quantity are required." });
            }

            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
            if (patient == null)
            {
                return BadRequest(new { Message = "Patient not found. Please register the patient first." });
            }

            var activeIngredient = await _context.ActiveIngredientVariants
                .Include(a => a.BaseIngredient)
                .Include(a => a.Trades) 
                .FirstOrDefaultAsync(a =>
                    a.BaseIngredient.IngredientName == IngredientName &&
                    a.Strength == strength &&
                    a.StrengthUnit == strengthUnit);

            if (activeIngredient == null)
            {
                return BadRequest(new { Message = "No active ingredient variant matches the specified strength and unit." });
            }

            var trade = activeIngredient.Trades.FirstOrDefault(t =>
                t.Name == dispensedMedication &&
                t.PharmaceuticalForm == form);

            if (trade == null)
            {
                return BadRequest(new { Message = "The dispensed medication does not match the specified active ingredient, strength, or form." });
            }

            if (trade.QuantityStock == null || trade.QuantityStock < quantity)
            {
                return BadRequest(new { Message = $"Not enough stock available. Available: {trade.QuantityStock ?? 0}" });
            }

            var existingPrescription = await _context.Prescriptions.FirstOrDefaultAsync(p =>
                p.PhoneNumber == phoneNumber &&
                p.Prescription_Description == prescriptionDescription &&
                p.Dose == dose &&
                p.Form == form &&
                p.IngredientName == IngredientName &&
                p.Dispensedmedication == dispensedMedication &&
                p.QuantityDispensed == quantity);

            if (existingPrescription != null)
            {
                return BadRequest(new { Message = "This prescription already exists." });
            }

            // تقليل الكمية من المخزون
            trade.QuantityStock -= quantity;

            var prescription = new Prescription
            {
                Prescription_Description = prescriptionDescription,
                Dose = dose,
                Form = form,
                PrescriptionDate = DateTime.UtcNow,
                Strength = strength,
                StrengthUnit = strengthUnit,
                Dispensedmedication = dispensedMedication,
                QuantityDispensed = quantity,
                PhoneNumber = phoneNumber,
                IngredientName = activeIngredient.BaseIngredient?.IngredientName,
                DrugBankID = activeIngredient.BaseIngredient?.DrugBankID 
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            var receipt = new
            {
                PatientName = patient.PatientName,
                PhoneNumber = patient.PhoneNumber,
                PrescriptionDescription = prescription.Prescription_Description,
                TradeName = trade.Name,
                Quantity = quantity,
                UnitPrice = trade.Price,
                TotalPrice = trade.Price.HasValue ? quantity * trade.Price.Value : 0,
                DispensedAt = DateTime.UtcNow
            };

            return Ok(new
            {
                Message = "Prescription added and medication dispensed successfully.",
                Receipt = receipt
            });
        }



        [HttpGet("all-prescriptions")]
        public async Task<IActionResult> GetAllPrescriptions()
        {
            var prescriptions = await _context.Prescriptions
                .Select(p => new
                {
                    p.Prescription_Description,
                    p.Dose,
                    p.Form,
                    p.PrescriptionDate,
                    p.DrugBankID,
                    p.Dispensedmedication,
                    p.QuantityDispensed,
                    p.PhoneNumber
                })
                .ToListAsync();

            if (!prescriptions.Any())
            {
                return NotFound(new { Message = "No prescriptions found." });
            }

            return Ok(prescriptions);
        }


        [HttpGet("prescriptions-by-date")]
        public async Task<IActionResult> GetPrescriptionsByDateRange(
    [FromQuery] DateTime startDate,
    [FromQuery] DateTime endDate)
        {
            var prescriptions = await _context.Prescriptions
                .Where(p => p.PrescriptionDate >= startDate && p.PrescriptionDate <= endDate)
                .Select(p => new
                {
                    p.Prescription_Description,
                    p.Dose,
                    p.Form,
                    p.PrescriptionDate,
                    p.DrugBankID,
                    p.Dispensedmedication,
                    p.QuantityDispensed,
                    p.PhoneNumber
                })
                .ToListAsync();

            if (!prescriptions.Any())
            {
                return NotFound(new { Message = "No prescriptions found in this date range." });
            }

            return Ok(prescriptions);
        }


        //    [HttpGet("total-sales")]
        //    public async Task<IActionResult> GetTotalSales()
        //    {
        //        var totalSales = await _context.Prescriptions
        //            .Join(_context.TradeNames,
        //                  p => p.Dispensedmedication,
        //                  t => t.Name,
        //                  (p, t) => new { p.QuantityDispensed, t.Price })
        //            .SumAsync(x => x.QuantityDispensed * x.Price);

        //        return Ok(new { TotalSales = totalSales });
        //    }

        //    [HttpGet("sales-by-date")]
        //    public async Task<IActionResult> GetSalesByDate(
        //[FromQuery] DateTime startDate,
        //[FromQuery] DateTime endDate)
        //    {
        //        var totalSales = await _context.Prescriptions
        //            .Where(p => p.PrescriptionDate >= startDate && p.PrescriptionDate <= endDate)
        //            .Join(_context.TradeNames,
        //                  p => p.Dispensedmedication,
        //                  t => t.Name,
        //                  (p, t) => new { p.QuantityDispensed, t.Price })
        //            .SumAsync(x => x.QuantityDispensed * x.Price);

        //        return Ok(new { TotalSales = totalSales });
        //    }

        //    [HttpGet("sales-by-medication/{medicationName}")]
        //    public async Task<IActionResult> GetSalesByMedication(string medicationName)
        //    {
        //        var totalSales = await _context.Prescriptions
        //            .Where(p => p.Dispensedmedication == medicationName)
        //            .Join(_context.TradeNames,
        //                  p => p.Dispensedmedication,
        //                  t => t.Name,
        //                  (p, t) => new { p.QuantityDispensed, t.Price })
        //            .SumAsync(x => x.QuantityDispensed * x.Price);

        //        return Ok(new { Medication = medicationName, TotalSales = totalSales });
        //    }


    }
}


