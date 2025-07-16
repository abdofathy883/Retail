using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ClockEntry
    {
        public Guid Id { get; set; }
        public Guid EmployeeProfileId { get; set; }
        public EmployeeProfile EmployeeProfile { get; set; }

        public DateTime ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }

        // Optional: store calculated work hours directly
        public double? TotalHours => ClockOut.HasValue
            ? (ClockOut.Value - ClockIn).TotalHours
            : null;
    }
}
