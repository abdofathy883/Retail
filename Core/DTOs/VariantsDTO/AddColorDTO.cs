using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.VariantsDTO
{
    public class AddColorDTO
    {
        public required string Name { get; set; }
        public required string ColorCode { get; set; }
    }
}
