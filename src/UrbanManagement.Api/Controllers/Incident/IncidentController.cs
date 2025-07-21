using Microsoft.AspNetCore.Mvc;

namespace UrbanManagement.Api.Controllers.Incident;

[ApiController]
[Route("api/incidents")]
public class IncidentController : ControllerBase
{
    // GET: api/incidents
    [HttpGet]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public IActionResult GetIncidents()
    {
        var incidents = new List<string> { "Chamado 1", "Chamado 2", "Chamado 3" };
        return Ok(incidents);
    }

    // GET: api/incidents/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetIncidentById(int id)
    {
        var incident = $"Chamado {id}";
        if (id <= 0)
        {
            return NotFound();
        }
        return Ok(incident);
    }

    // POST: api/incidents
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateIncident([FromBody] string incident)
    {
        if (string.IsNullOrEmpty(incident))
        {
            return BadRequest("incident cannot be empty.");
        }
        return CreatedAtAction(nameof(GetIncidentById), new { id = 1 }, incident);
    }

    // PUT: api/incidents/{id}
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateIncident(int id, [FromBody] string incident)
    {
        if (id <= 0 || string.IsNullOrEmpty(incident))
        {
            return BadRequest("Invalid incident data.");
        }
        return NoContent();
    }

    // DELETE: api/incidents/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteIncident(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }
        return NoContent();
    }
}
