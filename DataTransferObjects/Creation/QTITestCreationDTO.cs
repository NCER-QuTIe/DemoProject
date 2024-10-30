using Entities.Enums;

namespace DataTransferObjects.Creation;

public record QTITestCreationDTO
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? PackageBase64 { get; set; }

    public string[]? Tags { get; set; }

    public TestStatusEnum Status { get; set; }
}
