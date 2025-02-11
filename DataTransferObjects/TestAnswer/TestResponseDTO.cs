using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestAnswer;

public record TestResponseDTO
{
    // I use "TestId" (PascalCase) in place of testID.
    public string? TestId { get; set; }
    public long StartTimeEpoch { get; set; }
    public long EndTimeEpoch { get; set; }
    public List<ItemResponseDTO>? ItemResponses { get; set; }
}
