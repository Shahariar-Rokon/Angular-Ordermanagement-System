using OrderManagementClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.DTOsForAPI
{
    public class CustomerHeaderDTO()
    {
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; } = string.Empty;
        public string TableNo { get; set; } = string.Empty;
        public string DailyMenusCustomer { get; set; } = string.Empty;
        public bool IsCheckedOut { get; set; } = false;
    }
}
