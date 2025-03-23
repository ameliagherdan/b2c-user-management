namespace UserManagementSystem.Application.Interfaces;

public interface IGraphUserService
{
    Task<string> CreateUserAsync(string email, string password, string displayName);
}