namespace DataTransferObjects.TestResults;

public record ItemResponseDTO
{
    public int? ItemNumber { get; set; }

    public string? ItemIdentifier { get; set; }

    public Dictionary<string, string>? InteractionResponses { get; set; }

    public int DurationSeconds { get; set; }

    public PointsDTO? Points { get; set; }
}