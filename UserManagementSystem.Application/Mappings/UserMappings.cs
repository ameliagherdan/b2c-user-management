using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Domain.Entities;

namespace UserManagementSystem.Application.Mappings;

public static class UserMappings
{
    public static UserDto ToDto(this User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        FullName = user.FullName,
        Role = user.Role,
        CreatedAt = user.CreatedAt
    };

    public static User ToEntity(this CreateUserRequest request) => new()
    {
        Email = request.Email,
        FullName = request.FullName,
        Role = request.Role
    };
    
    public static void ApplyUpdate(this User user, UpdateUserRequest request)
    {
        user.Email = request.Email;
        user.FullName = request.FullName;
        user.Role = request.Role;
    }
}