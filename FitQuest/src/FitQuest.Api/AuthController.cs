using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FitQuest.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userMgr;
        private readonly IConfiguration _config;

        public AuthController(UserManager<User> userMgr, IConfiguration config)
        {
            _userMgr = userMgr;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new User { UserName = dto.Username, Email = dto.Email };
            var result = await _userMgr.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userMgr.FindByNameAsync(dto.Username);
            if (user == null || !await _userMgr.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            // Generate JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                // Add additional claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class User : IdentityUser<int>
    {
        // Additional properties can be added here if needed
    }
}