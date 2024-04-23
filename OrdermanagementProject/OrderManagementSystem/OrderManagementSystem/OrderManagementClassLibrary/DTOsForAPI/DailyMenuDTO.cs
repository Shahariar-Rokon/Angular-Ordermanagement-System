using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.DTOsForAPI
{
    public class DailyMenuDTO
    {
        public decimal DemandQuantity { get; set; }
        public int RecipeId { get; set; }
        public decimal Price { get; set; }
    }

    public class DailyMenuDTOAfter
    {
        public decimal CookedQuantity { get; set; } = 0;
        public decimal ServingQuantity { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }
}
