using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions.ExternalTest;

public class ExternalTestForCreationBadRequestException() : BadRequestException("ExternalTestForCreation object is null")
{
}