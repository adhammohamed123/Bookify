using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Exceptions
{
    public sealed class ConcurrencyException :Exception
    {
        public ConcurrencyException(string message,DBConcurrencyException innerException)
            :base(message,innerException)
        {
            
        }
    }
}
