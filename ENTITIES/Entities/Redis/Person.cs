using Redis.OM.Modeling;

namespace ENTITIES.Entities.Redis;

[Document(StorageType = StorageType.Json, Prefixes = new[] { "Person" })]
public class Person
{
    [RedisIdField] [Indexed] public string? Id { get; set; }

    [Indexed] public string? FirstName { get; set; }

    [Indexed] public string? LastName { get; set; }

    [Indexed] public int Age { get; set; }

    [Searchable] public string? PersonalStatement { get; set; }

    [Indexed] public string[] Skills { get; set; } = Array.Empty<string>();

    [Indexed(CascadeDepth = 1)] public Address? Address { get; set; }
}