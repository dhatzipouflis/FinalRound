using System.Text.Json.Serialization;
using FinalRound.Common.Api;
using FinalRound.Contracts.Api;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ErrorHandlingOptions>(builder.Configuration.GetSection("ErrorHandling"));
builder.Services.AddHttpContextAccessor();

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

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapSubscribeHandler();
app.MapControllers();
app.Run();
