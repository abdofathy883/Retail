using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class EmployeeProfile: ApplicationUser
    {
        public Guid Id { get; set; } // Same as AppUser.Id
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public decimal MonthlySalary { get; set; }
        public List<ClockEntry> ClockEntries { get; set; } = new();
    }
}
