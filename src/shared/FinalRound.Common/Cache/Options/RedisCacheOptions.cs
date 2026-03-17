namespace FinalRound.Common.Cache.Options;

public sealed class RedisCacheOptions
{
    public const string SectionName = "Redis";

    public string ConnectionString { get; set; } = string.Empty;
    public string InstanceName { get; set; } = "finalround:";
    public int DefaultExpirationMinutes { get; set; } = 10;
}