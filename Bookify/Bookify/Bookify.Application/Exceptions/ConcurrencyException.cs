using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Exceptions
{
    public sealed class ConcurrencyException(string message, Exception innerException) : Exception(message,innerException)
    {
    }
}
