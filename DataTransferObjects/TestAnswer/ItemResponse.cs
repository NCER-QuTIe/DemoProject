namespace DataTransferObjects.TestAnswer;

public record ItemResponse
{

    public int? ItemNumber { get; set; }

    public Dictionary<string, string>? ItemResponses { get; set; }

    public int DurationSeconds { get; set; }

    public Points? Points { get; set; }
}