using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementClassLibrary.Models;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleHeadersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SaleHeadersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SaleHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleHeader>>> GetSaleHeader()
        {
            return await _context.SaleHeader.Include(sh => sh.SaleDetails).ThenInclude(sd=> sd.DailyMenu).ToListAsync();
        }

        // GET: api/SaleHeaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleHeader>> GetSaleHeader(int id)
        {
            var saleHeader = await _context.SaleHeader.FindAsync(id);

            if (saleHeader == null)
            {
                return NotFound();
            }

            return saleHeader;
        }

        // PUT: api/SaleHeaders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleHeader(int id, SaleHeader saleHeader)
        {
            if (id != saleHeader.Id)
            {
                return BadRequest();
            }

            _context.Entry(saleHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleHeaderExists(id))
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

        // POST: api/SaleHeaders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SaleHeader>> PostSaleHeader(SaleHeader saleHeader)
        {
            try
            {
                saleHeader.TotalPrice = saleHeader.SaleDetails.Sum(s => s.Quantity * s.DailyMenu.Price);
                saleHeader.VAT = saleHeader.TotalPrice * 15 / 100;
                saleHeader.TotalBill = saleHeader.TotalPrice + saleHeader.VAT;

                List<DailyMenu> dailyMenus = new List<DailyMenu>();

                foreach (SaleDetails saleDetails in saleHeader.SaleDetails)
                {
                    DailyMenu dailyMenu = await _context.DailyMenus.SingleAsync(d => d.DailyMenuId == saleDetails.DailyMenuId);
                    dailyMenu.ServingQuantity += saleDetails.Quantity;
                    dailyMenus.Add(dailyMenu);
                }

                await _context.Database.BeginTransactionAsync();
                _context.SaleHeader.Add(saleHeader);
                _context.UpdateRange(dailyMenus);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return CreatedAtAction("GetSaleHeader", new { id = saleHeader.Id }, saleHeader);
            }
            catch(Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                return BadRequest();
            }
        }

        // DELETE: api/SaleHeaders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleHeader(int id)
        {
            var saleHeader = await _context.SaleHeader.FindAsync(id);
            if (saleHeader == null)
            {
                return NotFound();
            }

            _context.SaleHeader.Remove(saleHeader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleHeaderExists(int id)
        {
            return _context.SaleHeader.Any(e => e.Id == id);
        }
    }
}
