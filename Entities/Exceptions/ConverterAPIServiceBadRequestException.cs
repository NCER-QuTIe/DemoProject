using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public class ConverterAPIServiceBadRequestException : BadRequestException
{
    public ConverterAPIServiceBadRequestException() : base()
    {
    }

    public ConverterAPIServiceBadRequestException(string message) : base($"Provided package wasn't valid. {message}")
    {
        
    }

    public ConverterAPIServiceBadRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
