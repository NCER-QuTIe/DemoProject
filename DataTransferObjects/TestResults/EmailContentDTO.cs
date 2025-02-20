using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestResults;

public record EmailContentDTO
{
    public TestResponseBundleDTO? ResponseBundle { get; set; }
    public string? EmailToSend { get; set; }
}
