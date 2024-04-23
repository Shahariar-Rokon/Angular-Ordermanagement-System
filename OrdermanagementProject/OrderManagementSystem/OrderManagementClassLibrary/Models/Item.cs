using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.Models
{
    public class Item
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public string? Type { get; set; }

        public ICollection<RecipeItem>? RecipeItems { get; set; }
        public ICollection<Requisition>? Requisitions { get; set; }
    }
}
