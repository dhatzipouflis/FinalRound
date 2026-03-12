using FinalRound.Common.Api;
using FinalRound.Contracts.Api;
using Microsoft.AspNetCore.Mvc;

namespace FinalRound.Tournament.Api.Controllers;

[ApiController]
[Route("")]
public class TournamentController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => this.ApiOk(new PingResponse("tournament ok"));

    [HttpGet("boom")]
    public IActionResult Boom() => throw new InvalidOperationException("Boom from Tournament service");
}
