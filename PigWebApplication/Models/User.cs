using Microsoft.AspNetCore.Identity;

namespace PigWebApplication.Models
{
    public class User: IdentityUser
    {
        public int Year { get;set; }
    }
}
