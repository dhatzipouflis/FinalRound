using FinalRound.Common.Cache.Options;
using FinalRound.Common.Cache.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalRound.Common.Cache.Extensions;

public static class RedisCachingServiceCollectionExtensions
{
    public static IServiceCollection AddFinalRoundRedisCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RedisCacheOptions>(
            configuration.GetSection(RedisCacheOptions.SectionName));

        var redisOptions = configuration
                               .GetSection(RedisCacheOptions.SectionName)
                               .Get<RedisCacheOptions>()
                           ?? throw new InvalidOperationException("Missing Redis configuration.");

        if (string.IsNullOrWhiteSpace(redisOptions.ConnectionString))
        {
            throw new InvalidOperationException("Redis:ConnectionString is missing.");
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisOptions.ConnectionString;
            options.InstanceName = redisOptions.InstanceName;
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}