using GraphQLDemo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesController : ControllerBase
    {

        private readonly ILogger<ArticlesController> _logger;
        private readonly DbContextOptions<DemoContext> _options;

        public ArticlesController(ILogger<ArticlesController> logger, DbContextOptions<DemoContext> options)
        {
            _logger = logger;
            _options = options;
        }

        [HttpGet(Name = "GetArticles")]
        public IEnumerable<Article> Get()
        {
            using (var ctx = new DemoContext(_options))
            {
                return ctx.Articles.ToList();
            }
        }
    }
}