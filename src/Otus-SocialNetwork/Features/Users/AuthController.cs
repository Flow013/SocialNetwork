using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otus_SocialNetwork.Features.Users.Actions;
using Swashbuckle.AspNetCore.Annotations;

namespace Otus_SocialNetwork.Features.Users;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Упрощенный процесс аутентификации путем передачи идентификатор пользователя 
    /// и получения токена для дальнейшего прохождения авторизации
    /// </summary>
    [AllowAnonymous]
    [HttpPost("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, "Успешная аутентификация", typeof(AuthLoginCommand.AuthLoginResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка", typeof(string))]
    public async Task<IActionResult> Login(AuthLoginCommand.AuthLoginRequest request,
                                           CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (result.IsFailure)
        {
            return Ok(result.Error);
        }
        return Ok(result.Value);
    }
}
