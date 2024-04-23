using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.Models
{
    public class Requisition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequisitionId { get; set; }
        public string RequestedBy { get; set; } = string.Empty;
        public DateTime RequisitionDate { get; set; }
        public decimal RequestedQuantity { get; set; }
        //Recipe sum of (DemandQuantity of DailyMenu * Item Quantity of RecipeItem) Demand quantity 

        [ForeignKey(nameof(Item.ItemId))]
        public int ItemId { get; set; }
        public Item? Item { get; set; }
    }
}
