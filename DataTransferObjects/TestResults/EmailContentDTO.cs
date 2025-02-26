using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestResults;

public record EmailContentDTO
{
    required public TestResponseBundleDTO ResponseBundle { get; set; }
    required public string EmailToSend { get; set; }
}
