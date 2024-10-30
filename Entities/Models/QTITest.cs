using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using StackExchange.Redis;
using Redis.OM.Modeling;
using Redis.OM;

namespace Entities.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "QTITest" }, IndexName = "qtiTests", IdGenerationStrategyName = nameof(Uuid4IdGenerationStrategy))]
public class QTITest
{
    [RedisIdField]
    [Indexed]
    public Guid Id { get; set; }

    [Searchable]
    public string? Name { get; set; }

    [Searchable]
    public string? Description { get; set; }

    public string? PackageBase64 { get; set; }

    [Searchable]
    public string[]? Tags { get; set; }

    [Indexed]
    public TestStatusEnum Status { get; set; }

    public DateTime? Uploaded { get; set; }
}