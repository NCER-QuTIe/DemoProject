using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestAnswer;

public record EmailContentDTO
{
    public TestResponseBundleDTO? responseBundle { get; set; }
    public string? emailToSend { get; set; }
}
