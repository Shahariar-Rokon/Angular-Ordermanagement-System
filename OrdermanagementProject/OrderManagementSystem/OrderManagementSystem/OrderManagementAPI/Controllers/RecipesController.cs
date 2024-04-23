using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderManagementAPI.Data;
using OrderManagementClassLibrary.DTOsForAPI;
using OrderManagementClassLibrary.Models;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public RecipesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public System.Object GetAllRecipes()
        {
            var recipes = _db.Recipes.Include(r => r.RecipeItems)
                .Select(r => new
                {
                    r.RecipeId,
                    r.RecipeName,
                    RecipeItem = r.RecipeItems
                    .Select(ri => new
                    {
                        ri.RecipeItemId,
                        ri.Quantity,
                        ri.ItemId,
                    })
                });

            //List<Recipe> recipes = _db.Recipes.Include(r => r.RecipeItems).ToList();
            string jsonString = JsonConvert.SerializeObject(recipes, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
            return Content(jsonString,"application/json");
        }

        [HttpGet("{id}")]
        public IActionResult GetRecipeById(int id)
        {
            var recipes = _db.Recipes.Include(r => r.RecipeItems)
                .Where(r => r.RecipeId == id)
                .Select(r => new
                {
                    r.RecipeId,
                    r.RecipeName,
                    RecipeItem = r.RecipeItems
                    .Select(ri => new
                    {
                        ri.RecipeItemId,
                        ri.Quantity,
                        ri.ItemId,
                    })
                }).FirstOrDefault();
            //Recipe recipe = _db.Recipes.Include(r => r.RecipeItems).FirstOrDefault(r => r.RecipeId == id);
            if (recipes == null)
            {
                return NotFound("Recipe data not found.");
            }
            string jsonString = JsonConvert.SerializeObject(recipes, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
            return Content(jsonString,"application/json");
        }

        [HttpPost]
        public async Task<IActionResult> PostRecipe([FromForm] RecipeDTO recipeDTO)
        {
            Recipe recipe = new Recipe()
            {
                RecipeName = recipeDTO.RecipeName
            };
            _db.Recipes.Add(recipe);
            await _db.SaveChangesAsync();
            List<RecipeItem> recipeItems = JsonConvert.DeserializeObject<List<RecipeItem>>(recipeDTO.RecipeItems);
            var recipeObj = _db.Recipes.FirstOrDefault(r => r.RecipeName == recipeDTO.RecipeName);
            var recipeId = recipeObj.RecipeId;
            AddRecipeItem(recipeId, recipeItems);
            await _db.SaveChangesAsync();
            return Ok("Recipe saved successfully.");
        }

        private void AddRecipeItem(int recipeId, List<RecipeItem>? recipeItems)
        {
            if(recipeItems.Any())
            {
                foreach(var item in recipeItems)
                {
                    RecipeItem recipeItem = new RecipeItem()
                    {
                        RecipeId = recipeId,
                        Quantity = item.Quantity,
                        ItemId = item.ItemId,
                    };
                    _db.RecipeItems.Add(recipeItem);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, [FromForm] RecipeDTO recipeDTO)
        {
            var recipe = await _db.Recipes.FindAsync(id);
            if(recipe == null)
            {
                return NotFound("Recipe data not found.");
            }
            recipe.RecipeName = recipeDTO.RecipeName;
            var existingRecipeItem = _db.RecipeItems.Where(ri => ri.RecipeId == id);
            _db.RecipeItems.RemoveRange(existingRecipeItem);
            List<RecipeItem> list = JsonConvert.DeserializeObject<List<RecipeItem>>(recipeDTO.RecipeItems);
            AddRecipeItem(recipe.RecipeId, list);
            await _db.SaveChangesAsync();
            return Ok($"{id} no Recipe updated successfully.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _db.Recipes.FindAsync(id);
            if(recipe == null)
            {
                return BadRequest("Recipe data not found.");
            }

            if(recipe != null)
            {
                _db.Recipes.Remove(recipe);
                await _db.SaveChangesAsync();
            }

            return Ok($"{id} no Recipe deleted successfully.");
        }
    }
}
