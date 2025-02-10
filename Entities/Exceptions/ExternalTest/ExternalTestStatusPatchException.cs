using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions.ExternalTest;

public class ExternalTestStatusPatchException(Guid id, TestStatusEnum status, Exception e) : Exception($"Failed to update status of ExternalTest with id {id} to {status}; {e.Message}")
{
}
