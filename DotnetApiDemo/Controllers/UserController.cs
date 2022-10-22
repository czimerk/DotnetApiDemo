using AutoMapper;
using DotnetApiDemo;
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
        private readonly IMapper _mapper;

        public UsersController(ILogger<ArticlesController> logger, 
            DbContextOptions<DemoContext> options, 
            IConfiguration configuration, 
            IMapper mapper)
        {
            _logger = logger;
            _options = options;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<UserDto>> Get()
        {
            using (var ctx = new DemoContext(_options))
            {
                var users = await ctx.Users.ToListAsync();
                return users.Select(u => _mapper.Map<UserDto>(u));
            }
        }

        [HttpGet("me")]
        public UserDto GetMe()
        {
            var me = User.Identity;
            var name = me.Name;
            //var user = User.Identity as ClaimsIdentity;
            //var email = user.Claims
            //    .FirstOrDefault(c => c.Type == ClaimTypes.Name 
            //    && c.Value.Contains("@"))?.Value;
            User dbUser = null;
            using (var ctx = new DemoContext(_options))
            {
                dbUser = ctx.Users
                    .FirstOrDefault(x => x.Email == name);


            }
            return dbUser == null ? null : _mapper.Map<UserDto>(dbUser);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginDto user)
        {
            using (var ctx = new DemoContext(_options))
            {
                var users = await ctx.Users.ToListAsync();
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
