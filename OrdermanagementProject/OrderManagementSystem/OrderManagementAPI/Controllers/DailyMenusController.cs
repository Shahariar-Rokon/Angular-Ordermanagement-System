using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementClassLibrary.DTOsForAPI;
using OrderManagementClassLibrary.Models;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<DailyMenu>> PostDailyMenu(DailyMenuDTO dailyMenuDTO)
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
            return Ok(new { message = "Daily menu posted successfully." });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDailyMenu(int id, DailyMenuDTOAfter dailyMenuDTOAfter)
        {
            var dailyMenu = await _db.DailyMenus.FindAsync(id);
            dailyMenu.CookedQuantity = dailyMenuDTOAfter.CookedQuantity;
            //dailyMenu.ServingQuantity = dailyMenuDTOAfter.ServingQuantity;
            //dailyMenu.Price = dailyMenuDTOAfter.Price;
            await _db.SaveChangesAsync();
            return Ok(new {dailyMenu});
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>PutDailyMenu(int id,DailyMenu dailyMenu)
        {
            if (id != dailyMenu.DailyMenuId)
            {
                return BadRequest();
            }

            _db.Entry(dailyMenu).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyMenuExists(id))
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

        private bool DailyMenuExists(int id)
        {
            return _db.DailyMenus.Any(e => e.DailyMenuId == id);
        }
        //if (id == 0||dm==null)
        //{
        //    return BadRequest("No id/No Dailymenu");
        //}
        //DailyMenu dailyMenu = await _db.DailyMenus.FirstAsync(x=>x.DailyMenuId==id);
        //dailyMenu.DailyMenuDate = dm.DailyMenuDate;
        //dailyMenu.ServingQuantity = dm.ServingQuantity;
        //dailyMenu.CookedQuantity = dm.CookedQuantity;
        //dailyMenu.DemandQuantity = dm.DemandQuantity;
        //dailyMenu.Price = dm.Price;

        //return Ok(new { message = "Updated dailymenu" });
    
        

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

            return Ok(new {message= $"Daily menu {id} deleted successfully." });
        }
    }
}
