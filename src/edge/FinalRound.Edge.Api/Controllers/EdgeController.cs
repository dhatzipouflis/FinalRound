using FinalRound.Edge.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalRound.Edge.Api.Controllers;

[ApiController]
[Route("api/edge")]
public class EdgeController : ControllerBase
{
    private readonly ITournamentService _tournamentService;

    public EdgeController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    [HttpGet("tournament-ping")]
    public async Task<IActionResult> Ping()
    {
        var response  = await _tournamentService.PingAsync();
        return Ok(response);
    }

    [HttpGet("tournament-boom")]
    public async Task<IActionResult> Boom()
        => Ok(await _tournamentService.BoomAsync());
}
