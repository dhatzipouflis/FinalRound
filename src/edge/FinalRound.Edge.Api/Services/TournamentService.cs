using FinalRound.Contracts.Api;
using FinalRound.Edge.Api.Constants;
using FinalRound.Edge.Api.Invocation;

namespace FinalRound.Edge.Api.Services;

public sealed class TournamentService : ServiceInvocationServiceBase, ITournamentService
{
    public TournamentService(IServiceInvocationResolver resolver, IServiceInvocationClient client)
        : base(resolver, client)
    {
    }

    public Task<ApiResponse<PingResponse>> PingAsync()
        => InvokeAsync<PingResponse>(ServiceInvocationKeys.TournamentPing);

    public Task<ApiResponse<object>> BoomAsync()
        => InvokeAsync<object>(ServiceInvocationKeys.TournamentBoom);
}
