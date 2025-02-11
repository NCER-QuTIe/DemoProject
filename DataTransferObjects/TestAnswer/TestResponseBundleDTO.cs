using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestAnswer;

public record TestResponseBundleDTO
{
    public string? StudentName { get; set; }
    public List<TestResponseDTO>? TestResponses { get; set; }
}
