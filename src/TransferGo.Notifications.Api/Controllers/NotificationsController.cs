using MediatR;
using Microsoft.AspNetCore.Mvc;
using TranferGo.Notifications.Application.Commands.SendNotification.Contracts;
using TranferGo.Notifications.Application.Queries.GetUserNotifications.Contracts;

namespace TransferGo.Notifications.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("send")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpGet("user-notifications/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserNotifications([FromRoute] Guid userId)
    {
        var result = await _mediator.Send(new GetUserNotificationsQuery
        {
            UserId = userId
        });

        if (result.Count == 0)
        {
            return NotFound();
        }
        
        return Ok(result);
    }
}