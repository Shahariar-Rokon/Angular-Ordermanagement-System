using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderManagementAPI.Data;
using OrderManagementAPI.InvoiceGenerator;
using OrderManagementClassLibrary.DTOsForAPI;
using OrderManagementClassLibrary.Models;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseHeadersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PurchaseHeadersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerHeader>>> GetCustomerHeaders()
        {
            return await _context.CustomerHeaders.ToListAsync();
        }

        // GET: api/PurchaseHeaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerHeader>> GetCustomerHeader(int id)
        {
            var customerHeader = await _context.CustomerHeaders.FindAsync(id);

            if (customerHeader == null)
            {
                return NotFound();
            }

            return customerHeader;
        }

        // PUT: api/PurchaseHeaders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerHeader(int id, CustomerHeader customerHeader)
        {
            if (id != customerHeader.CustomerHeaderId)
            {
                return BadRequest();
            }

            _context.Entry(customerHeader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerHeaderExists(id))
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

      
        [HttpPost]
        public async Task<ActionResult> PostCustomerHeader([FromForm]CustomerHeaderDTO cdto)
        {
            CustomerHeader customer1 = new CustomerHeader
            {
                CustomerName = cdto.CustomerName,
                CustomerEmail = cdto.CustomerEmail,
                CustomerPhone = cdto.CustomerPhone,
                TableNo = cdto.TableNo,
                IsCheckedOut = cdto.IsCheckedOut
            };
            _context.CustomerHeaders.Add(customer1);
            await _context.SaveChangesAsync();   
            List<CustomersMenu> DailyMenuCustomerRecord = JsonConvert.DeserializeObject<List<CustomersMenu>>(cdto.DailyMenusCustomer);
            var customerObj = _context.CustomerHeaders.FirstOrDefault(x => x.CustomerName == cdto.CustomerName);
            var customerId = customerObj.CustomerHeaderId;
            AddBoughtMenu(customerId, DailyMenuCustomerRecord);
            await _context.SaveChangesAsync();
            return Ok("Customer posted")/*, new { id = customerHeader.CustomerHeaderId }, customerHeader*/;
        }

        private void AddBoughtMenu(int customerId, List<CustomersMenu>? DailyMenuCustomerRecord)
        {
            if (DailyMenuCustomerRecord.Any())
            {
                foreach(var item in DailyMenuCustomerRecord)
                {
                    CustomersMenu dm = new CustomersMenu
                    {
                        CustomerHeaderId = customerId,
                        DailyMenuId = item.DailyMenuId,
                        Quantity = item.Quantity,

                    };
                    _context.DailyMenuCustomerRecords.Add(dm);
                }
            }
        }

        // DELETE: api/PurchaseHeaders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerHeader(int id)
        {
            var customerHeader = await _context.CustomerHeaders.FindAsync(id);
            if (customerHeader == null)
            {
                return NotFound();
            }

            _context.CustomerHeaders.Remove(customerHeader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerHeaderExists(int id)
        {
            return _context.CustomerHeaders.Any(e => e.CustomerHeaderId == id);
        }
    }
}
