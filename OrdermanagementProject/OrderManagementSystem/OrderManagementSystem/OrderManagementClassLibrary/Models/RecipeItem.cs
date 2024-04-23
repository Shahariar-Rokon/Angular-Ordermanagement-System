using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementClassLibrary.Models
{
    public class RecipeItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipeItemId { get; set; }
        public decimal Quantity { get; set; }

        [ForeignKey(nameof(Recipe.RecipeId))]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        [ForeignKey(nameof(Item.ItemId))]
        public int ItemId { get; set; }
        public Item? Item { get; set; }
    }
}