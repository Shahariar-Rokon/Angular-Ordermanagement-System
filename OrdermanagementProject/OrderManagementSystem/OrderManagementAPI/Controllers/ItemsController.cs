using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementClassLibrary.Models;
using OrderManagementClassLibrary.DTOsForAPI;
using Microsoft.AspNetCore.Authorization;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ItemsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _db.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItemById(int id)
        {
            var item = await _db.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            return item;
        }


        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(ItemDTO itemDTO)
        {
            var existingItem = await _db.Items.FirstOrDefaultAsync(x => x.Name.ToLower() == itemDTO.Name.ToLower());
            if (existingItem != null)
            {
                return BadRequest("item already exists");
            }
            Item item = new Item()
            {
                Name = itemDTO.Name,
                Unit = itemDTO.Unit,
                Type = itemDTO.Type,
            };
            _db.Items.Add(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "A new Item addedd successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, ItemDTO itemDTO)
        {
            var item = await _db.Items.FindAsync(id);
            
            if(item == null)
            {
                return NotFound("Item not found.");
            }

            item.Name = itemDTO.Name;
            item.Unit = itemDTO.Unit;
            item.Type = itemDTO.Type;

            await _db.SaveChangesAsync();
            return Ok(new {message="Item update successful"});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            if(item != null)
            {
                _db.Items.Remove(item);
                await _db.SaveChangesAsync();
            }

            return Ok(new { message = $"{id} no Item deleted successfully." });
        }
    }
}
