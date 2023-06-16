using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Entities
{
    public class UserDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [StringLength(8)]
        public string Password { get; set; }
        [StringLength(8)]
        [Compare("Password")]
        public string PasswordConfirmed { get; set; }
    }
}
