using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestResults;

public record TestResponseDTO
{
    required public Guid TestId { get; set; }
    public long StartTimeEpoch { get; set; }
    public long EndTimeEpoch { get; set; }
    required public List<ItemResponseDTO>? ItemResponses { get; set; }
}
