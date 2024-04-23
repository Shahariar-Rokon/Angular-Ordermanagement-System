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

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ItemsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Getitems()
        {
            return await _db.Items.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _db.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, [FromForm] ItemDTO itemDTO)
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
            return Ok($"{id} no Item updated successfully." + item);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> PostItem([FromForm] ItemDTO itemDTO)
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
            _db.SaveChanges();
            return Ok("A new Item addedd successfully.");
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

            return Ok($"{id} no Item deleted successfully.");
        }
    }
}
