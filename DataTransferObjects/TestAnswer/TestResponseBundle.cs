using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestAnswer;

public record TestResponseBundle
{
    public string? StudentName { get; set; }
    required public List<TestResponse> TestResponses { get; set; }
}
