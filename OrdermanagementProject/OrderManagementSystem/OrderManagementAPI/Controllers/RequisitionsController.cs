using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementClassLibrary.DTOsForAPI;
using OrderManagementClassLibrary.Models;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequisitionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequisitionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Requisitions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Requisition>>> GetRequisitions()
        {
            return await _context.Requisitions.ToListAsync();
        }

        // GET: api/Requisitions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Requisition>> GetRequisition(int id)
        {
            var requisition = await _context.Requisitions.FindAsync(id);

            if (requisition == null)
            {
                return NotFound();
            }

            return requisition;
        }

        // PUT: api/Requisitions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequisition(int id, Requisition requisition)
        {
            if (id != requisition.RequisitionId)
            {
                return BadRequest();
            }

            _context.Entry(requisition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequisitionExists(id))
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

        // POST: api/Requisitions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Requisition>> PostRequisition([FromBody]MenuDate MenuDate)
        {
            var findMenu = _context.DailyMenus.FirstOrDefault(x => x.DailyMenuDate == MenuDate.DailyMenuDate);
            if (findMenu == null)
            {
                throw new Exception("Daily Menu Date not found enter again");
            }
            List<DailyMenu> dailyMenuList = await _context.DailyMenus.Where(d => d.DailyMenuDate == MenuDate.DailyMenuDate).ToListAsync();
            Console.WriteLine(dailyMenuList.Count());
            int itemId = 0;
            decimal demandQuantity = 0;
            List<Requisition> requisitionList = new List<Requisition>();
            foreach (DailyMenu dailyMenu in dailyMenuList)
            {
                Recipe aRecipe = _context.Recipes.Single(r => r.RecipeId == dailyMenu.RecipeId);
                List<RecipeItem> itemList = _context.RecipeItems.Where(ri => ri.RecipeId == aRecipe.RecipeId).ToList();
                foreach (RecipeItem recipeItem in itemList)
                {
                    itemId = recipeItem.ItemId;
                    demandQuantity = dailyMenu.DemandQuantity * recipeItem.Quantity;

                    if (requisitionList.Any(r => r.ItemId == itemId))
                    {
                        requisitionList.Single(r => r.ItemId == itemId).RequestedQuantity += demandQuantity;
                    }
                    else
                    {
                        requisitionList.Add(new Requisition { ItemId = itemId, RequestedQuantity = demandQuantity, RequisitionDate = MenuDate.DailyMenuDate });//added menu date to req date
                    }

                }
            }
            _context.Requisitions.AddRange(requisitionList);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Added Req" });/*CreatedAtAction("GetRequisition", new { id = requisition.RequisitionId }, requisition)*/;
        }

        // DELETE: api/Requisitions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequisition(int id)
        {
            var requisition = await _context.Requisitions.FindAsync(id);
            if (requisition == null)
            {
                return NotFound();
            }

            _context.Requisitions.Remove(requisition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequisitionExists(int id)
        {
            return _context.Requisitions.Any(e => e.RequisitionId == id);
        }
    }
}
