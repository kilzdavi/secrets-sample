using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookStoreCore.Data
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(255)]
        public string FirstName { get; set; }
        [MaxLength(255)]
        public string LastName { get; set; }
    }
}
