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
    }
}
