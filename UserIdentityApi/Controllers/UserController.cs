using IdentityApi.Context;
using IdentityApi.Entities;
using IdentityApi.Jwt;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly GetToken _token;

        public UserController(AppDbContext context, GetToken token)
        {
            _context = context;
            _token = token;
        }

        [HttpPost]
        [Route("/Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(dto.Password != dto.PasswordConfirmed) 
            {
                return BadRequest("Password no same");
            }

            var user = dto.Adapt<User>();

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        [HttpPost]
        [Route("/GetToken")]
        public async Task<IActionResult> GetToken([FromBody] ForToken toke)
        {
            if (toke == null) { return BadRequest(ModelState); }

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var user = await _context.Users.FirstOrDefaultAsync(p=>p.UserName == toke.UserName);
            
            if(user == null || user.Password != toke.Password)
            {
                return NotFound();
            }

            var token = _token.Token(user);

            return Ok(token);
        }

        [HttpPost]
        [Route("getuser")]
        public async Task<IActionResult> GetUserByUserName([FromForm] string username)
        {
            if(username == null)
            {
                return BadRequest();
            }

            if(!_context.Users.Any(x => x.UserName == username))
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            return Ok(user);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }

            if (!_context.Users.Any(x => x.UserName == username))
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            _ = _context.Users.Remove(user!);
            return Ok("removed");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(string username, [FromForm] UserDto dto)
        {

            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.Password != dto.PasswordConfirmed)
            {
                return BadRequest("Password no same");
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            user = dto.Adapt<User>();
            
            _context.Users.Update(user);  
            _context.SaveChanges();
            return Ok(user);
        }
        [HttpGet]
        [Route("/Profile")]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FirstOrDefaultAsync(p=>p.Id== userid);
            if (user == null) { return NotFound(); }

            return Ok(user);
        }
    }
}
