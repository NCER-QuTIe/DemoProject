using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.TestResults;

public record PointsDTO
{
    public int Received { get; set; }
    public int Maximal { get; set; }
}