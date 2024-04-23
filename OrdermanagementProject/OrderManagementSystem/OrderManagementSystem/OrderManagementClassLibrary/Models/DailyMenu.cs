using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.Models
{
    public class DailyMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DailyMenuId { get; set; }
        [DataType(DataType.Date), Column(TypeName = "date")]
        public DateTime DailyMenuDate { get; set; }
        public decimal DemandQuantity { get; set; }
        public decimal CookedQuantity { get; set; }
        public decimal ServingQuantity { get; set; }
        public decimal Price { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public ICollection<SaleDetails> SaleDetails { get; set; }
    }
}
