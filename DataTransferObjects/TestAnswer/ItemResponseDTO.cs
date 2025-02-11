namespace DataTransferObjects.TestAnswer;

public record ItemResponseDTO
{

    // Optional property
    public int? ItemNumber { get; set; }

    // The key/value pairs from TypeScript's { [key: string]: string }
    public Dictionary<string, string>? ItemResponses { get; set; }

    public int DurationSeconds { get; set; }
    public PointDTO? Points { get; set; }
}