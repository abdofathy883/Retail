using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class ObjectAlreadyExistsException: Exception
    {
        public ObjectAlreadyExistsException(string message) : base(message) { }
    }
}
