using FinalRound.Contracts.Api;

namespace FinalRound.Edge.Api.Services;

public interface ITournamentService
{
    Task<ApiResponse<PingResponse>> PingAsync();
    Task<ApiResponse<object>> BoomAsync();
}
