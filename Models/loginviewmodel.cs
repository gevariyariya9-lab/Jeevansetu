
using System.ComponentModel.DataAnnotations;

namespace JeevanSetu.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Please enter your username.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your password.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a login type.")]
    public string Role { get; set; } = string.Empty;
}
