using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestResults;

public record TestResponseBundleDTO
{
    required public string? StudentName { get; set; }
    required public List<TestResponseDTO> TestResponses { get; set; }
}
