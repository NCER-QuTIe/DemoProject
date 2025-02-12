using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestAnswer;

public record Points
{
    public int Received { get; set; }
    public int Maximal { get; set; }
}