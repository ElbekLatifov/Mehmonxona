using System.ComponentModel.DataAnnotations;

namespace HotelsBlazor.Entities.Models;

public class UserDto
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmed { get; set; }
}
