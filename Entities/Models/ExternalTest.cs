using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using StackExchange.Redis;
using Redis.OM.Modeling;
using Redis.OM;

namespace Entities.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "ExternalTest" }, IndexName = "externalTests", IdGenerationStrategyName = nameof(Uuid4IdGenerationStrategy))]
public class ExternalTest
{
    [RedisIdField]
    [Indexed]
    public Guid Id { get; set; }

    [Searchable]
    public string? Name { get; set; }

    [Searchable]
    public string? Description { get; set; }

    public string? Url { get; set; }

    [Searchable]
    public string[]? Tags { get; set; }

    [Indexed]
    public TestStatusEnum Status { get; set; }

    public DateTime? Uploaded { get; set; }
}