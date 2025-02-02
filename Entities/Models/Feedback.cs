using Redis.OM.Modeling;
using Redis.OM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "Feedback" }, IndexName = "Feedbacks", IdGenerationStrategyName = nameof(Uuid4IdGenerationStrategy))]
public class Feedback
{
    [RedisIdField]
    [Indexed]
    public Guid Id { get; set; }
    
    [Searchable]
    public string? Email { get; set; }
    
    [Searchable]
    [Required]
    public string Message { get; set; }
    
    public DateTime? Uploaded { get; set; }
}
