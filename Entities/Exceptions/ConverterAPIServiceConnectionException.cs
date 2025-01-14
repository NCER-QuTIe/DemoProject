using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public class ConverterAPIServiceConnectionException : ConverterAPIServiceException
{
    public ConverterAPIServiceConnectionException(Exception e) : base("Error while connecting to the Converter API Service", e)
    {
    }

    public ConverterAPIServiceConnectionException(string message, Exception e) : base(message, e)
    {
    }

    public ConverterAPIServiceConnectionException(string message) : base(message)
    {
    }

    public ConverterAPIServiceConnectionException() : base()
    {
    }
}