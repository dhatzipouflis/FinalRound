using FinalRound.Common.Api;
using FinalRound.Contracts.Api;
using Microsoft.AspNetCore.Mvc;

namespace FinalRound.Match.Api.Controllers;

[ApiController]
[Route("")]
public class MatchController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => this.ApiOk(new PingResponse("match ok"));
}
