using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.Models
{
    public class SaleDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(SaleHeader))]
        public int SaleHeaderId {  get; set; }

        [ForeignKey(nameof(DailyMenu))]
        public int DailyMenuId { get; set; }

        public int Quantity { get; set; }

        public SaleHeader? SaleHeader { get; set; }
        public DailyMenu? DailyMenu { get; set; }
    }
}
