using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Application.DTOs;

public class UpdateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [RegularExpression("Admin|User")]
    public string Role { get; set; } = "User";
}