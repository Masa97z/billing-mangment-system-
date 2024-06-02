using billing_mangment_system.Models;
using billing_mangment_system_v2.Dtos;
using billing_mangment_system_v2.ICollectionService;
using billing_mangment_system_v2.Models;
using Microsoft.AspNetCore.Mvc;

namespace billing_mangment_system_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmuntController : Controller
    {
        private readonly IAmount _amunt;
        public AmuntController(IAmount amunt)
        {
            _amunt = amunt;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoces>>> GetInvoces()
        {
            var inv = await _amunt.GetInvocesAsync();
            return Ok(inv);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoces>> GetInvocesById(string id)
        {
            var inv = await _amunt.GetInvocesByIdAsync(id);
            if (inv == null)
            {
                return NotFound();
            }
            return Ok(inv);
        }

        [HttpPost]
        public async Task<ActionResult<Invoces>> CreateInvoces(Invoces invoce)
        {
            try
            {
                var newInv = await _amunt.CreateInvocesAsync(invoce);
                return CreatedAtAction(nameof(GetInvocesById), new { id = newInv.Id }, newInv);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while creating the invoces.");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvoces(string id, Invoces inv)
        {
            if (id != inv.CostumerId)
            {
                return BadRequest("User ID mismatch.");
            }
            try
            {


                await _amunt.UpdateInvocesAsync(id, inv);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the invoces.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvoces(string id)
        {
            try
            {
                await _amunt.DeleteInvocesAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while deleting the invoces.");
            }
        }
        [HttpPut("new/{id}")]
        public async Task<ActionResult> UpdateInvocesNew(string id, AmountDto inv)
        {
            if (id != inv.CostumerId)
            {
                return BadRequest("User ID mismatch.");
            }
            var invoc = await _amunt.GetInvocesByIdAsync(id);
            var currentinv = invoc.FirstOrDefault(u => u.CostumerId == id);
            var update = new Invoces
            {
                Id = currentinv.Id,
                CostumerId = id,
                TotalAmount = currentinv.TotalAmount-inv.TotalAmount,
                CurrentAmount = currentinv.CurrentAmount,
                Deps = currentinv.Deps,

            };
            try
            {


                await _amunt.UpdateInvocesAsync(id, update);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while updating the invoces.");
            }
        }
    }
}
