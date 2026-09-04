using API.FurnitureStore.API.Configuration;
using API.FurnitureStore.Share.Auth;
using API.FurnitureStore.Share.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.FurnitureStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly JWTConfig _jwtConfig;

        public AuthenticationController(UserManager<IdentityUser> userManager, IOptions<JWTConfig> jwtConfig)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig.Value;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDTO request)
        {
            // Implementation for user registration
            if (!ModelState.IsValid) return BadRequest();

            var emailExists = await _userManager.FindByEmailAsync(request.Email);
            if (emailExists != null) return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "Email already exists"
                }
            });

            //Create user
            var user = new IdentityUser()
            {
                Email = request.Email,
                UserName = request.Email
            };

            var isCreated = await _userManager.CreateAsync(user, request.Password);

            if (isCreated.Succeeded)
            {
                var token = GenerateToken(user);
                return Ok(new AuthResult()
                {
                    Result = true,
                    Token = token
                });
            }
            else
            {
                var errors = new List<string>();
                foreach (var err in isCreated.Errors)
                    errors.Add(err.Description);

                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = errors
                });
            }

            return BadRequest(new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "User couldn't be created"
                }
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();

            //check if user exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
                return BadRequest(new AuthResult
                {
                    Errors = new List<string> { "invalid Payload" },
                    Result = false
                });

            var checkUserAndPass = await _userManager.CheckPasswordAsync(existingUser, request.Password);
            if (!checkUserAndPass)
                return BadRequest(new AuthResult
                {
                    Errors = new List<string> { "invalid Credentials" },
                    Result = false
                });

            var token = GenerateToken(existingUser);
            return Ok(new AuthResult
            {
                Token = token,
                Result = true
            });
        }

        private string GenerateToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                })),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
