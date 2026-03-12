using FinalRound.Common.Api;
using FinalRound.Contracts.Api;
using Microsoft.AspNetCore.Mvc;

namespace FinalRound.Identity.Api.Controllers;

[ApiController]
[Route("")]
public class IdentityController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => this.ApiOk(new PingResponse("identity ok"));
}
