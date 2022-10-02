using DotnetApiDemo.Domain;
using DotnetApiDemo.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GraphQLDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ArticlesController> _logger;
        private readonly DbContextOptions<DemoContext> _options;

        public UsersController(ILogger<ArticlesController> logger, DbContextOptions<DemoContext> options, IConfiguration configuration)
        {
            _logger = logger;
            _options = options;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> Get()
        {
            using (var ctx = new DemoContext(_options))
            {
                return ctx.Users.ToList();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            using (var ctx = new DemoContext(_options))
            {
                var users = ctx.Users.ToList();
                if (users.Any(u => u.Email == user.Username && u.Password == user.Password))
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var token = CreateToken(authClaims);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }
            return Unauthorized();
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,
                SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
