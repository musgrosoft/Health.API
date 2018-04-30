 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthAPI;
 using Repositories;
 using Repositories.Models;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/AlcoholIntakes1")]
    public class AlcoholIntakes1Controller : Controller
    {
        private readonly HealthContext _context;

        public AlcoholIntakes1Controller(HealthContext context)
        {
            _context = context;
        }

        // GET: api/AlcoholIntakes1
        [HttpGet]
        public IEnumerable<AlcoholIntake> GetAlcoholIntakes()
        {
            return _context.AlcoholIntakes;
        }

        // GET: api/AlcoholIntakes1/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAlcoholIntake([FromRoute] DateTime id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var alcoholIntake = await _context.AlcoholIntakes.SingleOrDefaultAsync(m => m.DateTime == id);

            if (alcoholIntake == null)
            {
                return NotFound();
            }

            return Ok(alcoholIntake);
        }

        // PUT: api/AlcoholIntakes1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlcoholIntake([FromRoute] DateTime id, [FromBody] AlcoholIntake alcoholIntake)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != alcoholIntake.DateTime)
            {
                return BadRequest();
            }

            _context.Entry(alcoholIntake).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlcoholIntakeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AlcoholIntakes1
        [HttpPost]
        public async Task<IActionResult> PostAlcoholIntake([FromBody] AlcoholIntake alcoholIntake)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AlcoholIntakes.Add(alcoholIntake);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AlcoholIntakeExists(alcoholIntake.DateTime))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAlcoholIntake", new { id = alcoholIntake.DateTime }, alcoholIntake);
        }

        // DELETE: api/AlcoholIntakes1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlcoholIntake([FromRoute] DateTime id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var alcoholIntake = await _context.AlcoholIntakes.SingleOrDefaultAsync(m => m.DateTime == id);
            if (alcoholIntake == null)
            {
                return NotFound();
            }

            _context.AlcoholIntakes.Remove(alcoholIntake);
            await _context.SaveChangesAsync();

            return Ok(alcoholIntake);
        }

        private bool AlcoholIntakeExists(DateTime id)
        {
            return _context.AlcoholIntakes.Any(e => e.DateTime == id);
        }
    }
}