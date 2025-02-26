namespace DataTransferObjects.TestResults;

public record ItemResponseDTO
{
    required public int ItemNumber { get; set; }

    required public string? ItemIdentifier { get; set; }

    required public Dictionary<string, List<string> >? InteractionResponses { get; set; }

    required public int DurationSeconds { get; set; }

    required public PointsDTO? Points { get; set; }
}