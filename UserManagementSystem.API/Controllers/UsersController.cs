using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Application.Mappings;

namespace UserManagementSystem.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
[Consumes("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns>List of users.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users.Select(u => u.ToDto()));
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    /// <param name="id">User GUID.</param>
    /// <returns>User object.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetByIdAsync([FromRoute] Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user is null)
            return NotFound();

        return Ok(user.ToDto());
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <param name="request">Create user input.</param>
    /// <returns>Created user with location header.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> CreateAsync([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = request.ToEntity();
        var id = await _userService.CreateAsync(user);

        return CreatedAtAction(nameof(GetByIdAsync), new { id }, user.ToDto());
    }

    /// <summary>
    /// Update a user by ID.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="request">Updated user info.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.GetByIdAsync(id);
        if (user is null)
            return NotFound();

        user.ApplyUpdate(request);

        var success = await _userService.UpdateAsync(user);
        return success ? NoContent() : StatusCode(500, "Failed to update user");
    }

    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    /// <param name="id">User ID.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var success = await _userService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}