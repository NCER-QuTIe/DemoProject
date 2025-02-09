using Entities.Enums;

namespace DataTransferObjects.Creation;

public record ExternalTestCreationDTO
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Url { get; set; }

    public string[]? Tags { get; set; }

    public TestStatusEnum Status { get; set; }
}
