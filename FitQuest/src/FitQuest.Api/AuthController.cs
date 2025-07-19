using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User = FitQuest.Shared.Models.User;

namespace FitQuest.Api
{
    /// <summary>
    /// Authentication controller responsible for user registration and login operations.
    /// Handles JWT token generation and user identity management using ASP.NET Core Identity.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // ASP.NET Core Identity UserManager for user operations (create, find, authenticate)
        private readonly UserManager<User> _userMgr;
        
        // Configuration service for accessing JWT settings from appsettings.json
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes the authentication controller with required dependencies.
        /// </summary>
        /// <param name="userMgr">UserManager for handling user operations</param>
        /// <param name="config">Configuration service for JWT settings</param>
        public AuthController(UserManager<User> userMgr, IConfiguration config)
        {
            _userMgr = userMgr;
            _config = config;
        }

        /// <summary>
        /// Registers a new user in the FitQuest application.
        /// Creates a new user account with username, email, and password.
        /// </summary>
        /// <param name="dto">Registration data containing username, email, and password</param>
        /// <returns>Ok() if registration successful, BadRequest() with errors if failed</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Create new user instance with provided credentials
            var user = new User { UserName = dto.Username, Email = dto.Email };
            
            // Attempt to create the user with ASP.NET Core Identity
            var result = await _userMgr.CreateAsync(user, dto.Password);
            
            // Return appropriate response based on registration result
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok();
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token for API access.
        /// Validates credentials and creates a token with multiple claim types for compatibility.
        /// The token includes various user ID claims to support different authentication scenarios.
        /// </summary>
        /// <param name="dto">Login credentials containing username and password</param>
        /// <returns>JWT token if authentication successful, Unauthorized if failed</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // Find user by username in the Identity system
            var user = await _userMgr.FindByNameAsync(dto.Username);
            
            // Validate user exists and password is correct
            if (user == null || !await _userMgr.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            // Ensure username is not null for JWT generation (safety check)
            if (string.IsNullOrEmpty(user.UserName))
            {
                throw new InvalidOperationException("UserName cannot be null or empty.");
            }

            // Create JWT claims with multiple user ID formats for maximum compatibility
            // This supports different client-side authentication scenarios and claim requirements
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),                 // Standard name claim
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // ASP.NET Core standard user ID
                new Claim("sub", user.Id.ToString()),                      // JWT standard subject claim
                new Claim("id", user.Id.ToString()),                       // Alternative ID claim for client compatibility
                // Additional claims can be added here as application features expand
            };

            // Retrieve JWT configuration settings with validation
            var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var jwtIssuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            var jwtAudience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");

            // Create signing credentials using HMAC SHA256 algorithm
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate JWT token with 24-hour expiration
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(1),  // Token valid for 24 hours
                signingCredentials: creds);

            // Return the serialized JWT token to the client
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

    /// <summary>
    /// Data transfer object for user registration containing required user information.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>Gets or sets the desired username for the new account.</summary>
        public required string Username { get; set; }
        
        /// <summary>Gets or sets the email address for the new account.</summary>
        public required string Email { get; set; }
        
        /// <summary>Gets or sets the password for the new account.</summary>
        public required string Password { get; set; }
    }

    /// <summary>
    /// Data transfer object for user login containing authentication credentials.
    /// </summary>
    public class LoginDto
    {
        /// <summary>Gets or sets the username for authentication.</summary>
        public required string Username { get; set; }
        
        /// <summary>Gets or sets the password for authentication.</summary>
        public required string Password { get; set; }
    }
}