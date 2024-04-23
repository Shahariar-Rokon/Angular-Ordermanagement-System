using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.InvoiceGenerator;
using OrderManagementClassLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

            return await _context.SaleHeader.Include(x => x.SaleDetails).ThenInclude(x => x.DailyMenu).ThenInclude(x=>x.Recipe).ToListAsync();
        }

        // GET: api/SaleHeaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleHeader>> GetSaleHeader(int id)
        {
            var saleHeader = await _context.SaleHeader.Include(x => x.SaleDetails).ThenInclude(x => x.DailyMenu).ThenInclude(x => x.Recipe).FirstOrDefaultAsync(x=>x.Id==id);

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

            try
            {
                if (saleHeader.SaleDetails == null || !saleHeader.SaleDetails.Any())
                {
                    throw new Exception("At least one sale detail is required.");
                }

                var existingSaleHeader = await _context.SaleHeader.FindAsync(id);

                if (existingSaleHeader == null)
                {
                    return NotFound();
                }

                existingSaleHeader.CustomerName = saleHeader.CustomerName;
                existingSaleHeader.CustomerEmail = saleHeader.CustomerEmail;
                existingSaleHeader.CustomerPhone = saleHeader.CustomerPhone;
                existingSaleHeader.SaleDate = saleHeader.SaleDate;

                // Remove existing SaleDetails entities
                List<SaleDetails> existingSaleDetailsToRemove = _context.SaleDetails.Where(x => x.SaleHeaderId == existingSaleHeader.Id).ToList();
                foreach (var saleDetail in existingSaleDetailsToRemove)
                {
                    _context.SaleDetails.Remove(saleDetail);
                }

                // Add new SaleDetails entities
                List<SaleDetails> newSaleDetails = new List<SaleDetails>();
                foreach (SaleDetails saleDetails in saleHeader.SaleDetails)
                {
                    saleDetails.DailyMenu = await _context.DailyMenus
                        .SingleAsync(d => d.DailyMenuId == saleDetails.DailyMenuId);
                    newSaleDetails.Add(saleDetails);
                }
                existingSaleHeader.SaleDetails = newSaleDetails;

                existingSaleHeader.TotalPrice = existingSaleHeader.SaleDetails.Sum(s => s.Quantity * s.DailyMenu.Price);
                existingSaleHeader.VAT = existingSaleHeader.TotalPrice * 15 / 100;
                existingSaleHeader.TotalBill = existingSaleHeader.TotalPrice + existingSaleHeader.VAT;

                List<DailyMenu> dailyMenus = new List<DailyMenu>();
                foreach (SaleDetails saleDetails in existingSaleHeader.SaleDetails)
                {
                    DailyMenu dailyMenu = await _context.DailyMenus.SingleAsync(d => d.DailyMenuId == saleDetails.DailyMenuId);
                    dailyMenu.ServingQuantity += saleDetails.Quantity;
                    dailyMenus.Add(dailyMenu);
                }

                await _context.Database.BeginTransactionAsync();
                _context.Entry(existingSaleHeader).State = EntityState.Modified;
                _context.UpdateRange(dailyMenus);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                return BadRequest(ex.Message);
            }
        }
        //public async Task<IActionResult> PutSaleHeader(int id, SaleHeader saleHeader)
        //{
        //    if (id != saleHeader.Id)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        if (saleHeader.SaleDetails == null || !saleHeader.SaleDetails.Any())
        //        {
        //            throw new Exception("At least one sale detail is required.");
        //        }

        //        var existingSaleHeader = await _context.SaleHeader.FindAsync(id);

        //        if (existingSaleHeader == null)
        //        {
        //            return NotFound();
        //        }

        //        existingSaleHeader.CustomerName = saleHeader.CustomerName;
        //        existingSaleHeader.CustomerEmail = saleHeader.CustomerEmail;
        //        existingSaleHeader.CustomerPhone = saleHeader.CustomerPhone;
        //        existingSaleHeader.SaleDate = saleHeader.SaleDate;

        //        // Remove existing SaleDetails entities
        //        List<SaleDetails> existingSaleDetailsToRemove = _context.SaleDetails.Where(x => x.SaleHeaderId == existingSaleHeader.Id).ToList();
        //        foreach (var saleDetail in existingSaleDetailsToRemove)
        //        {
        //            _context.SaleDetails.Remove(saleDetail);
        //        }

        //        // Add new SaleDetails entities
        //        foreach (SaleDetails saleDetails in saleHeader.SaleDetails)
        //        {
        //            saleDetails.DailyMenu = await _context.DailyMenus.SingleAsync(d => d.DailyMenuId == saleDetails.DailyMenuId);
        //            _context.SaleDetails.Add(saleDetails);
        //        }

        //        existingSaleHeader.TotalPrice = existingSaleHeader.SaleDetails.Sum(s => s.Quantity * s.DailyMenu.Price);
        //        existingSaleHeader.VAT = existingSaleHeader.TotalPrice * 15 / 100;
        //        existingSaleHeader.TotalBill = existingSaleHeader.TotalPrice + existingSaleHeader.VAT;

        //        List<DailyMenu> dailyMenus = new List<DailyMenu>();
        //        foreach (SaleDetails saleDetails in existingSaleHeader.SaleDetails)
        //        {
        //            DailyMenu dailyMenu = await _context.DailyMenus.SingleAsync(d => d.DailyMenuId == saleDetails.DailyMenuId);
        //            dailyMenu.ServingQuantity += saleDetails.Quantity;
        //            dailyMenus.Add(dailyMenu);
        //        }

        //        await _context.Database.BeginTransactionAsync();
        //        _context.Entry(existingSaleHeader).State = EntityState.Modified;
        //        _context.UpdateRange(dailyMenus);
        //        await _context.SaveChangesAsync();
        //        await _context.Database.CommitTransactionAsync();

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _context.Database.RollbackTransactionAsync();
        //        return BadRequest(ex.Message);
        //    }
        //}
        //public async Task<IActionResult> PutSaleHeader(int id, SaleHeader saleHeader)
        //{
        //    if (id != saleHeader.Id)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        if (saleHeader.SaleDetails == null || !saleHeader.SaleDetails.Any())
        //        {
        //            throw new Exception("At least one sale detail is required.");
        //        }
        //        var existingSaleHeader = await _context.SaleHeader.FindAsync(id);
        //        if (existingSaleHeader == null)
        //        {
        //            return NotFound();
        //        }
        //        existingSaleHeader.CustomerName = saleHeader.CustomerName;
        //        existingSaleHeader.CustomerEmail = saleHeader.CustomerEmail;
        //        existingSaleHeader.CustomerPhone = saleHeader.CustomerPhone;
        //        existingSaleHeader.SaleDate = saleHeader.SaleDate;
        //        existingSaleHeader.SaleDetails = saleHeader.SaleDetails;
        //        foreach (SaleDetails saleDetails in existingSaleHeader.SaleDetails)
        //        {
        //            saleDetails.DailyMenu = await _context.DailyMenus.SingleAsync(d => d.DailyMenuId == saleDetails.DailyMenuId);
        //        }
        //        existingSaleHeader.TotalPrice = existingSaleHeader.SaleDetails.Sum(s => s.Quantity * s.DailyMenu.Price);
        //        existingSaleHeader.VAT = existingSaleHeader.TotalPrice * 15 / 100;
        //        existingSaleHeader.TotalBill = existingSaleHeader.TotalPrice + existingSaleHeader.VAT;
        //        List<DailyMenu> dailyMenus = new List<DailyMenu>();
        //        foreach (SaleDetails saleDetails in existingSaleHeader.SaleDetails)
        //        {
        //            DailyMenu dailyMenu = await _context.DailyMenus.SingleAsync(d => d.DailyMenuId == saleDetails.DailyMenuId);
        //            dailyMenu.ServingQuantity += saleDetails.Quantity;
        //            dailyMenus.Add(dailyMenu);
        //        }
        //        await _context.Database.BeginTransactionAsync();
        //        _context.Entry(existingSaleHeader).State = EntityState.Modified;
        //        _context.UpdateRange(dailyMenus);
        //        await _context.SaveChangesAsync();
        //        await _context.Database.CommitTransactionAsync();

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        await _context.Database.RollbackTransactionAsync();
        //        return BadRequest(ex.Message);
        //    }
        //}

        // POST: api/SaleHeaders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SaleHeader>> PostSaleHeader(SaleHeader saleHeader)
        {
            try
            {
                if (saleHeader.SaleDetails == null || !saleHeader.SaleDetails.Any())
                {
                    throw new Exception("At least one sale detail is required.");
                }
                foreach (SaleDetails saleDetails in saleHeader.SaleDetails)
                {
                    saleDetails.DailyMenu = await _context.DailyMenus.SingleAsync(d => d.DailyMenuId == saleDetails.DailyMenuId);
                }
                saleHeader.TotalPrice = saleHeader.SaleDetails.Sum(s => s.Quantity * s.DailyMenu.Price);
                saleHeader.VAT = saleHeader.TotalPrice * 15 / 100;
                saleHeader.TotalBill = saleHeader.TotalPrice + saleHeader.VAT;
                saleHeader.InvoiceNumber = InvoiceNumber.Get();

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
