using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsBlazor.Entities.Models;

[Table("users")]
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
