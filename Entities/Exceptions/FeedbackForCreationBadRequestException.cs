using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;

public class FeedbackForCreationBadRequestException() : BadRequestException("FeedbackForCreation object is null")
{
}