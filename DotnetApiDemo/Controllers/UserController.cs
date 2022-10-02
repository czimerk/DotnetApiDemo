using DotnetApiDemo.Dto;
using GraphQLDemo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsersController : ControllerBase
    {

        private readonly ILogger<ArticlesController> _logger;
        private readonly DbContextOptions<DemoContext> _options;

        public UsersController(ILogger<ArticlesController> logger, DbContextOptions<DemoContext> options)
        {
            _logger = logger;
            _options = options;
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> Get()
        {
            using (var ctx = new DemoContext(_options))
            {
                return ctx.Users.ToList();
            }
        }

        [HttpPost(Name = "GetUsers")]
        [Route("login")]
        public IEnumerable<string> CreateToken(UserLoginDto user)
        {
            using (var ctx = new DemoContext(_options))
            {
                var users = ctx.Users.ToList();
                if (users.Any(u => u.Email == user.Username && u.Password == user.Password))
                { 
                
                }
            }
            return "";
        }
    }
}
