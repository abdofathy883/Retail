using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ApplicationUser: IdentityUser, IAuditable, IDeletable
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        //public EmployeeProfile? Employee { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
