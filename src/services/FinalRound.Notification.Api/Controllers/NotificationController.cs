using FinalRound.Common.Api;
using FinalRound.Contracts.Api;
using Microsoft.AspNetCore.Mvc;

namespace FinalRound.Notification.Api.Controllers;

[ApiController]
[Route("")]
public class NotificationController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => this.ApiOk(new PingResponse("notification ok"));
}
