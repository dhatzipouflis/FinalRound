using FinalRound.Common.Cache.Options;
using Microsoft.Extensions.Options;

namespace FinalRound.Common.Cache.Services;

public interface ICacheKeyBuilder
{
    string Shared(params object[] parts);
    string Service(params object[] parts);
}

public sealed class CacheKeyBuilder : ICacheKeyBuilder
{
    private readonly RedisCacheOptions _options;

    public CacheKeyBuilder(IOptions<RedisCacheOptions> options)
    {
        _options = options.Value;
    }

    public string Shared(params object[] parts)
        => Build(_options.Namespaces.Shared, parts);

    public string Service(params object[] parts)
        => Build(_options.Namespaces.Service, parts);

    private string Build(string scope, params object[] parts)
    {
        var normalizedParts = parts
            .Where(x => x is not null)
            .Select(x => x!.ToString()!.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(Sanitize);

        return $"{Sanitize(_options.Namespaces.Root)}:{Sanitize(scope)}:{string.Join(":", normalizedParts)}";
    }

    private static string Sanitize(string value)
        => value.Replace(" ", "-", StringComparison.Ordinal).ToLowerInvariant();
}