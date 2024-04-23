using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.Models
{
    public class SaleHeader
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerEmail { get; set; }

        public string? CustomerPhone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SaleDate { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal VAT { get; set; }

        public decimal TotalBill { get; set; }

        public ICollection<SaleDetails> SaleDetails { get; set; }
    }
}
