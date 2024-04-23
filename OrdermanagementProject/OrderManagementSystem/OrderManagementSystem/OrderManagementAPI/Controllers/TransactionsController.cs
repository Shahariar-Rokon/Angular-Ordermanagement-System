using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Data;
using OrderManagementAPI.InvoiceGenerator;
using OrderManagementClassLibrary.DTOsForAPI;
using OrderManagementClassLibrary.Models;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private AppDbContext _db;

        public TransactionsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromForm] TransactionDTO tdto)
        {
            try
            {
                var Customer = _db.CustomerHeaders.Where(x => x.TableNo == tdto.TableNo && x.IsCheckedOut == false).FirstOrDefault();
                var dailyMenuRecords = _db.DailyMenuCustomerRecords.Where(x => x.CustomerHeaderId == Customer.CustomerHeaderId);

                decimal total = 0;
                if (dailyMenuRecords != null)
                {
                    foreach (var record in dailyMenuRecords)
                    {

                        var dailyMenu = await _db.DailyMenus.FindAsync(record.DailyMenuId);
                        total += dailyMenu.Price * record.Quantity;
                    }
                }
                Transaction transaction = new Transaction()
                {
                    TableNo = tdto.TableNo,
                    InvoiceNumber = InvoiceNumber.Get(),
                    TotalPay = total
                };
                _db.Transactions.Add(transaction);
                Customer.IsCheckedOut = true;
                await _db.SaveChangesAsync();
                return Ok("Transaction Complete");
            }
            catch(Exception ex)
            {
                return BadRequest("Customer already CheckedOut");
            } 
        }
    }
}
