using Microsoft.AspNetCore.Mvc;
using UrbanManagement.Api.Responses;
using UrbanManagement.Application.DTOs;
using UrbanManagement.Application.Interfaces;

namespace UrbanManagement.Api.Controllers.Incident;

[ApiController]
[Route("api/v1/incidents")]
public class IncidentController : ControllerBase
{
    private readonly IIncidentService _incidentService;
    private readonly ILogger<IncidentController> _logger;

    public IncidentController(IIncidentService incidentService, ILogger<IncidentController> logger)
    {
        _incidentService = incidentService ?? throw new ArgumentNullException(nameof(incidentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<IncidentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncidents(CancellationToken cancellationToken)
    {
        var incidents = await _incidentService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<IncidentDto>>.Success(incidents));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IncidentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<IncidentDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetIncidentById(Guid id, CancellationToken cancellationToken)
    {
        var incident = await _incidentService.GetByIdAsync(id, cancellationToken);
        if (incident is null)
        {
            return NotFound(ApiResponse<IncidentDto>.Failure(new ApiError("INCIDENT_NOT_FOUND", $"Incidente '{id}' não encontrado.")));
        }

        return Ok(ApiResponse<IncidentDto>.Success(incident));
    }

    [HttpGet("area/{areaCode}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<IncidentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncidentsByArea(string areaCode, CancellationToken cancellationToken)
    {
        var incidents = await _incidentService.GetByAreaAsync(areaCode, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<IncidentDto>>.Success(incidents));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<IncidentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<IncidentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateIncident([FromBody] CreateIncidentRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(ApiResponse<IncidentDto>.Failure(new ApiError("INVALID_PAYLOAD", "Payload inválido.")));
        }

        try
        {
            var incident = await _incidentService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetIncidentById), new { id = incident.Id }, ApiResponse<IncidentDto>.Success(incident));
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Falha ao criar incidente.");
            return BadRequest(ApiResponse<IncidentDto>.Failure(new ApiError("INVALID_INCIDENT_DATA", ex.Message)));
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateIncident(Guid id, [FromBody] UpdateIncidentRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(ApiResponse<object>.Failure(new ApiError("INVALID_PAYLOAD", "Payload inválido.")));
        }

        try
        {
            await _incidentService.UpdateAsync(id, request, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Falha ao atualizar incidente {IncidentId}.", id);
            return BadRequest(ApiResponse<object>.Failure(new ApiError("INVALID_INCIDENT_DATA", ex.Message)));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex, "Incidente {IncidentId} não encontrado.", id);
            return NotFound(ApiResponse<object>.Failure(new ApiError("INCIDENT_NOT_FOUND", ex.Message)));
        }
    }

    [HttpPatch("{id:guid}/assignment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignIncident(Guid id, [FromBody] AssignIncidentRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest(ApiResponse<object>.Failure(new ApiError("INVALID_PAYLOAD", "Payload inválido.")));
        }

        try
        {
            await _incidentService.AssignAsync(id, request, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Falha ao atribuir incidente {IncidentId}.", id);
            return BadRequest(ApiResponse<object>.Failure(new ApiError("INVALID_ASSIGNMENT_DATA", ex.Message)));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Usuário não autorizado a atribuir incidente {IncidentId}.", id);
            return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<object>.Failure(new ApiError("ASSIGNMENT_NOT_ALLOWED", ex.Message)));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Regra de atribuição inválida para incidente {IncidentId}.", id);
            return BadRequest(ApiResponse<object>.Failure(new ApiError("INVALID_ASSIGNMENT_RULE", ex.Message)));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex, "Entidade não encontrada ao atribuir incidente {IncidentId}.", id);
            return NotFound(ApiResponse<object>.Failure(new ApiError("ENTITY_NOT_FOUND", ex.Message)));
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteIncident(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _incidentService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex, "Incidente {IncidentId} não encontrado para exclusão.", id);
            return NotFound(ApiResponse<object>.Failure(new ApiError("INCIDENT_NOT_FOUND", ex.Message)));
        }
    }
}
