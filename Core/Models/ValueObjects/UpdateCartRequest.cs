using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ValueObjects
{
    public class UpdateCartRequest
    {
        public CartOwner CartOwner { get; set; }
        public UpdateCartDTO UpdateCartDTO { get; set; }
    }
}
