using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.VariantsDTO
{
    public class UpdateColorDTO
    {
        public int Id { get; set; }
        public string NewName { get; set; }
        public string NewColorCode { get; set; }
    }
}
