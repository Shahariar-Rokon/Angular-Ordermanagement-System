using Microsoft.AspNetCore.Identity;

namespace OrderManagementAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
