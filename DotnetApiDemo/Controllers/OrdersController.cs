using GraphQLDemo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraphQLDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class OrdersController : ControllerBase
    {

        private readonly ILogger<ArticlesController> _logger;
        private readonly DbContextOptions<DemoContext> _options;

        public OrdersController(ILogger<ArticlesController> logger, DbContextOptions<DemoContext> options)
        {
            _logger = logger;
            _options = options;
        }

        [HttpGet(Name = "GetOrders")]
        public IEnumerable<Order> Get()
        {
            using (var ctx = new DemoContext(_options))
            {
                return ctx.Orders.Include(o => o.User)
                    .Include(o => o.OrderLines)
                    .ThenInclude(p => p.Article).ToList();

            }
        }
    }
}
