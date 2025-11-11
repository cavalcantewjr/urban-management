using Microsoft.AspNetCore.Mvc;
using UrbanManagement.Api.Responses;
using UrbanManagement.Application.DTOs;
using UrbanManagement.Application.Interfaces;

namespace UrbanManagement.Api.Controllers.User;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<UserDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<UserDto>>.Success(users));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return NotFound(ApiResponse<UserDto>.Failure(new ApiError("USER_NOT_FOUND", $"Usuário '{id}' não encontrado.")));
        }

        return Ok(ApiResponse<UserDto>.Success(user));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(ApiResponse<UserDto>.Failure(new ApiError("INVALID_PAYLOAD", "Payload inválido.")));
        }

        try
        {
            var created = await _userService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = created.Id }, ApiResponse<UserDto>.Success(created));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Falha ao criar usuário.");
            return BadRequest(ApiResponse<UserDto>.Failure(new ApiError("INVALID_USER_DATA", ex.Message)));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Falha ao criar usuário por violação de regra de negócio.");
            return BadRequest(ApiResponse<UserDto>.Failure(new ApiError("USER_ALREADY_EXISTS", ex.Message)));
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(ApiResponse<object>.Failure(new ApiError("INVALID_PAYLOAD", "Payload inválido.")));
        }

        try
        {
            await _userService.UpdateAsync(id, request, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Falha ao atualizar usuário {UserId}.", id);
            return BadRequest(ApiResponse<object>.Failure(new ApiError("INVALID_USER_DATA", ex.Message)));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex, "Usuário {UserId} não encontrado.", id);
            return NotFound(ApiResponse<object>.Failure(new ApiError("USER_NOT_FOUND", ex.Message)));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflito ao atualizar usuário {UserId}.", id);
            return BadRequest(ApiResponse<object>.Failure(new ApiError("USER_ALREADY_EXISTS", ex.Message)));
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex, "Usuário {UserId} não encontrado para exclusão.", id);
            return NotFound(ApiResponse<object>.Failure(new ApiError("USER_NOT_FOUND", ex.Message)));
        }
    }
}
