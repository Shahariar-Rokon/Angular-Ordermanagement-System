using OrderManagementClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.DTOsForAPI
{
    public class SaleHeaderDTO
    {
        public SaleHeader SaleHeader { get; set; }
        public List<SaleDetails> SaleDetails { get; set; }
    }
}
