using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Application.Interfaces;

namespace UserManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class AuthController : ControllerBase
{
    private readonly IGraphUserService _graphUserService;
    private readonly IAuthService _authService;

    public AuthController(IGraphUserService graphUserService, IAuthService authService)
    {
        _graphUserService = graphUserService;
        _authService = authService;
    }

    /// <summary>
    /// Get the current authenticated user's identity from the JWT token.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        var email = User.FindFirstValue(ClaimTypes.Email) ?? "unknown";
        var name = User.FindFirstValue("name") ?? "unknown";
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "N/A";

        var claims = User.Claims.Select(c => new { c.Type, c.Value });

        return Ok(new
        {
            Email = email,
            Name = name,
            Role = role,
            Claims = claims
        });
    }

    /// <summary>
    /// Registers a new Azure AD B2C user via Microsoft Graph API.
    /// </summary>
    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = await _graphUserService.CreateUserAsync(
            request.Email,
            request.Password,
            request.FullName);

        return Ok(new { UserId = userId });
    }

    /// <summary>
    /// Login using username and password via ROPC (Resource Owner Password Credential) flow.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var (success, token, error) = await _authService.LoginAsync(request);

        return success
            ? Ok(JsonSerializer.Deserialize<object>(token!))
            : BadRequest(new { error = "Login failed", details = error });
    }
}
