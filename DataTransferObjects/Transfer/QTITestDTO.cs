using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Transfer;

public record QTITestDTO
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? PackageBase64 { get; set; }

    public string[]? Tags { get; set; }

    public TestStatusEnum Status { get; set; }

    public DateTime? Uploaded { get; set; }
}
