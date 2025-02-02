using Redis.OM.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Transfer;

public record FeedbackDTO
{
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public DateTime? Uploaded { get; set; }
}
