using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public class ConverterAPIServiceException : Exception
{
    public ConverterAPIServiceException(string message, Exception e) : base(message, e)
    {
    }

    public ConverterAPIServiceException(string message) : base(message)
    {
    }

    public ConverterAPIServiceException() : base()
    {
    }
}
