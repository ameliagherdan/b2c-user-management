using UserManagementSystem.Application.DTOs;

namespace UserManagementSystem.Application.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginRequest request);
}