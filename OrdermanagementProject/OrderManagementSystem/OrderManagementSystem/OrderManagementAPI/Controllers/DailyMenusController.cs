using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class DailyMenusController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DailyMenusController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyMenu>>> GetDailyMenus()
        {
            //var dm = await _db.DailyMenus.ToListAsync();
            //return Ok(new { dm });

            var result = (from d in _db.DailyMenus
                          join r in _db.Recipes on d.RecipeId equals r.RecipeId
                          select new
                          {
                              d.DailyMenuId,
                              d.DailyMenuDate,
                              d.DemandQuantity,
                              d.CookedQuantity,
                              d.ServingQuantity,
                              d.Price,
                              d.Recipe
                          }).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DailyMenu>> GetDailyMenu(int id)
        {
            var dm = await _db.DailyMenus.FirstOrDefaultAsync(x => x.DailyMenuId == id);
            if (dm != null)
            {
                var rc = (from r in _db.Recipes
                          join i in _db.RecipeItems on r.RecipeId equals i.RecipeId
                          where (r.RecipeId == dm.RecipeId)
                          select new
                          {
                              r.RecipeId,
                              r.RecipeName,
                              r.RecipeItems
                          });
                return Ok(new { dm, rc });

            }
            else
            {
                return BadRequest();
            }

            //var dailyMenu = (from d in _db.DailyMenus
            //                 join r in _db.Recipes on d.RecipeId equals r.RecipeId
            //                 where d.DailyMenuId == id
            //                 select new
            //                 {
            //                     d.DailyMenuId,
            //                     d.DailyMenuDate,
            //                     d.DemandQuantity,
            //                     d.CookedQuantity,
            //                     d.ServingQuantity,
            //                     d.Price,
            //                     d.Recipe
            //                 }).ToList();
            //return Ok(dailyMenu);
        }

        [HttpPost]
        public async Task<ActionResult<DailyMenu>> PostDailyMenu([FromForm] DailyMenuDTO dailyMenuDTO)
        {
            DailyMenu dailyMenu = new DailyMenu()
            {
                DailyMenuDate = DateTime.Today,
                DemandQuantity = dailyMenuDTO.DemandQuantity,
                Price = dailyMenuDTO.Price,
                RecipeId = dailyMenuDTO.RecipeId
            };
            _db.DailyMenus.Add(dailyMenu);
            await _db.SaveChangesAsync();
            return Ok("Daily menu posted successfully.");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDailyMenu(int id, [FromForm] DailyMenuDTOAfter dailyMenuDTOAfter)
        {
            var dailyMenu = await _db.DailyMenus.FindAsync(id);
            dailyMenu.CookedQuantity = dailyMenuDTOAfter.CookedQuantity;
            dailyMenu.ServingQuantity = dailyMenuDTOAfter.ServingQuantity;
            dailyMenu.Price = dailyMenuDTOAfter.Price;
            await _db.SaveChangesAsync();
            return Ok(new {dailyMenu});
        }
        //[HttpPost]
        //public async Task<IActionResult>PatchMenu(List<>)

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDailyMenu(int id)
        {
            var dailyMenu = await _db.DailyMenus.FindAsync(id);
            if (dailyMenu == null)
            {
                return NotFound();
            }

            if(dailyMenu != null)
            {
                _db.DailyMenus.Remove(dailyMenu);
                await _db.SaveChangesAsync();
            }

            return Ok($"Daily menu {id} deleted successfully.");
        }
    }
}
