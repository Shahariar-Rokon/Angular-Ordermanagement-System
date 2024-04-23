using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace OrderManagementClassLibrary.Models
{
    public class CustomerHeader()
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerHeaderId { get; set; }
       
        [AllowNull]
        public string CustomerName { get; set; } = string.Empty;
        [AllowNull]
        public string CustomerPhone { get; set; } =string.Empty;
        [DataType(DataType.EmailAddress)]
        [AllowNull]
        public string CustomerEmail { get; set; } = string.Empty;
        public string TableNo { get; set; } = string.Empty;
        public bool IsCheckedOut { get; set; }= false;

        public ICollection<DailyMenu>? DailyMenus { get; set; } = new List<DailyMenu>();
    }
}
