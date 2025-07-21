using Microsoft.AspNetCore.Mvc;

namespace UrbanManagement.Api.Controllers.User;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = new List<string> { "Usuario 1", "Usuario 2", "Usuario 3" };
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }
        var usuario = $"Usuario {id}";
        return Ok(usuario);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] string usuario)
    {
        if (string.IsNullOrEmpty(usuario))
        {
            return BadRequest("Usuario cannot be empty.");
        }
        return CreatedAtAction(nameof(GetUserById), new { id = 1 }, usuario);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] string usuario)
    {
        if (id <= 0 || string.IsNullOrEmpty(usuario))
        {
            return BadRequest("Invalid user data.");
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        if (id <= 0)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("hello")]
    public IActionResult Hello()
    {
        return Ok("Hello, this is the UserController! =)");
    }
}
