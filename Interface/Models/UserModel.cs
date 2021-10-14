using System.ComponentModel.DataAnnotations;

namespace Interface.Models;

public class UserModel
{
    public string? Id { get; set; }

    [Required]
    [MinLength(3)]
    public string Username { get; set; }

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? FirstName { get; set; }

    [MaxLength(30)]
    public string? LastName { get; set; }

    public string? ProfilePhoto { get; set; }

    public string[]? Roles { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? Created { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime[]? AuthenticationHistory { get; set; }

    public string[]? ApplicationsUsed { get; set; }
}