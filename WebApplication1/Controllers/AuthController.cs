using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SERVER_SIDE.DBContext;
using SERVER_SIDE.Models;
using SERVER_SIDE.Models.DTOModels;
using SERVER_SIDE.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SERVER_SIDE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataBaseContext _context;
        private readonly MembersService _memberService; // Service dependency injection

        public AuthController(IConfiguration configuration, DataBaseContext context, MembersService memberService)
        {
            _configuration = configuration;
            _context = context;
            _memberService = memberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMembersThatRegistered()
        {
            var allMembers = await _memberService.GetAllMembers();
            return Ok(allMembers);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] MemberLogin loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid request body.");
            }

            // Validate credentials using the DTO
            var member = _context.memberEntity.FirstOrDefault(u => u._name == loginRequest._name);
            if (member == null || !BCrypt.Net.BCrypt.Verify(loginRequest._passwordHash, member._passwordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate JWT
            var token = GenerateJwtToken(member);

            // Create and return the response
            var response = new ResponseModel(token, member._name, member._role);
            return Ok(response);
        }

        private string GenerateJwtToken(Member member)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey), "JWT secret key is not configured.");
            }

            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("name", member._name),
                    new Claim("role", member._role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}