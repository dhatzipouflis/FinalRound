namespace FinalRound.Common.Cache.Options;

public sealed class RedisCacheOptions
{
    public const string SectionName = "Redis";

    public string ConnectionString { get; set; } = string.Empty;
    public int DefaultExpirationMinutes { get; set; } = 10;
    public RedisNamespaces Namespaces { get; set; } = new();
}

public sealed class RedisNamespaces
{
    public string Root { get; set; } = "finalround";
    public string Shared { get; set; } = "shared";
    public string Service { get; set; } = "service";
}