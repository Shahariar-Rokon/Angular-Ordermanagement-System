using OrderManagementClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementClassLibrary.DTOsForAPI
{
    public class RecipeDTO
    {
        public string? RecipeName { get; set; }

        public string? RecipeItems { get; set; }
    }
}
