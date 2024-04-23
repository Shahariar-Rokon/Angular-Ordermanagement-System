using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.Models
{
    public class CustomersMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DailyMenuCustomerRecordId { get; set; }
        public decimal Quantity { get; set; }

        [ForeignKey(nameof(CustomerHeader.CustomerHeaderId))]
        public int CustomerHeaderId { get; set; }
        public CustomerHeader? CustomerHeader { get; set; }

        [ForeignKey(nameof(DailyMenu.DailyMenuId))]
        public int DailyMenuId { get; set; }
        public DailyMenu? DailyMenu { get; set; }
    }
}
