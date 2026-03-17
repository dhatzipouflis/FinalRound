using System.Text.Json.Serialization;
using FinalRound.Common.Api;
using FinalRound.Common.Cache.Extensions;
using FinalRound.Contracts.Api;
using FinalRound.Edge.Api.Invocation;
using FinalRound.Edge.Api.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ErrorHandlingOptions>(builder.Configuration.GetSection("ErrorHandling"));
builder.Services.AddHttpContextAccessor();

builder.Services.AddFinalRoundRedisCaching(builder.Configuration);

builder.Services.Configure<ServiceInvocationOptions>(builder.Configuration.GetSection("ServiceInvocation"));
builder.Services.AddSingleton<IServiceInvocationResolver, ServiceInvocationResolver>();
builder.Services.AddScoped<IServiceInvocationClient, ServiceInvocationClient>();
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services
    .AddControllers()
    .AddDapr()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.DictionaryKeyPolicy = null;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddDaprClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.InvalidModelStateResponseFactory = ctx =>
    {
        var correlationId = ctx.HttpContext.Request.Headers.TryGetValue(CorrelationTraceMiddleware.CorrelationHeader, out var v)
            ? v.ToString()
            : Guid.NewGuid().ToString("N");

        var errors = ctx.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors.Select(e =>
                new ApiError("validation_error", string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Validation error" : e.ErrorMessage)))
            .ToArray();

        return new OkObjectResult(ApiResponse<object>.Fail(errors, ctx.HttpContext.TraceIdentifier, correlationId));
    };
});

var app = builder.Build();

app.UseCorrelationAndTraceHeaders();
app.UseApiExceptionHandling();
if (!app.Environment.IsDevelopment()) app.UseHsts();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapSubscribeHandler();
app.MapControllers();
app.Run();
