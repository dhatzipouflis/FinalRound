using FinalRound.Common.Api;
using FinalRound.Contracts.Api;
using Microsoft.AspNetCore.Mvc;

namespace FinalRound.Ranking.Api.Controllers;

[ApiController]
[Route("")]
public class RankingController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => this.ApiOk(new PingResponse("ranking ok"));
}
