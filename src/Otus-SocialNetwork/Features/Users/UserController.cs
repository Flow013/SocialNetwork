using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otus_SocialNetwork.Features.Users.Actions;
using OtusSocialNetwork.DataClasses.Dtos;
using OtusSocialNetwork.DataClasses.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace Otus_SocialNetwork.Features.Users;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Регистрация нового пользователя
    /// </summary>
    [AllowAnonymous]
    [HttpPost("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, "Успешная регистрация", typeof(AuthRegisterCommand.AuthRegisterResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка", typeof(string))]
    public async Task<IActionResult> Register(AuthRegisterCommand.AuthRegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Получение анкеты пользователя
    /// </summary>
    [HttpGet("get/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение анкеты пользователя", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка", typeof(ErrorResponse))]
    public async Task<IActionResult> GetUser(UsersGetUserQuery.UsersGetUserQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(new ErrorResponse(result.Error));
        }
        return Ok(result.Value);
    }

    /// <summary>
    /// Поиск анкет
    /// </summary>
    [HttpGet("[action]")]
    [SwaggerResponse(StatusCodes.Status200OK, "Успешное получение списка анкет пользователей", typeof(UserDto[]))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Ошибка", typeof(ErrorResponse))]
    public async Task<IActionResult> Search(UsersSearchQuery.UsersSearchQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(new ErrorResponse(result.Error));
        }
        return Ok(result.Value);
    }
}