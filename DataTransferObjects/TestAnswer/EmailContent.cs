using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestAnswer;

public record EmailContent
{
    public TestResponseBundle? ResponseBundle { get; set; }
    public string? EmailToSend { get; set; }
}
