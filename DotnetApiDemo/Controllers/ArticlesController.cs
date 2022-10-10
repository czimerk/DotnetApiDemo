using AutoMapper;
using DotnetApiDemo.Dto;
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
        private readonly IMapper _mapper;
        private readonly DbContextOptions<DemoContext> _options;

        public ArticlesController(ILogger<ArticlesController> logger, DbContextOptions<DemoContext> options, IMapper mapper)
        {
            _logger = logger;
            _options = options;
            _mapper = mapper;
        }

        //Nem változtatja a Route-ot
        [HttpGet(Name = "GetArticles")]
        public IEnumerable<Article> Get([FromQuery] FilterDto filter)
        {
            using (var ctx = new DemoContext(_options))
            {
                var hasNameFilter = !string.IsNullOrWhiteSpace(filter.Name);
                return ctx.Articles.Where(a =>
                    (hasNameFilter ? a.Name.Contains(filter.Name) : true) &&
                    (!filter.PriceLessThan.HasValue || a.Price < filter.PriceLessThan) &&
                    (!filter.PriceGreaterThan.HasValue || a.Price > filter.PriceGreaterThan))
                    .ToList();

            }
        }


        [HttpGet]
        [Route("{id}")]
        public Article GetById(Guid id)
        {
            using (var ctx = new DemoContext(_options))
            {
                return ctx.Articles.FirstOrDefault(a => a.Id == id);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ArticleDto>> UpdateArticle(Guid id, [FromBody] ArticleDto articleDto)
        {
            if (id == Guid.Empty || articleDto.Id == Guid.Empty)
                return BadRequest($"Invalid {nameof(articleDto.Id)}");
            using (var ctx = new DemoContext(_options))
            {
                var dbArticle = ctx.Articles.FirstOrDefault(a => a.Id == id);
                if (dbArticle == null)
                    return BadRequest($"Invalid {nameof(articleDto.Id)}");
                dbArticle.Name = articleDto.Name;
                dbArticle.Price = articleDto.Price;
                dbArticle.Unit = articleDto.Unit;

                var res = ctx.Update(dbArticle);
                await ctx.SaveChangesAsync(); //fontos különben nem jutnak érvényre a változtatások
                var updatedArticle = _mapper.Map<ArticleDto>(res.Entity);
                return updatedArticle;
            }
        }

        [HttpPost]
        public async Task<ArticleDto> AddArticle([FromBody] ArticleDto articleDto)
        {
            using (var ctx = new DemoContext(_options))
            {
                var newArticle = new Article()
                {
                    Name = articleDto.Name,
                    Price = articleDto.Price,
                    Unit = articleDto.Unit,
                    Created = DateTime.Now
                };

                var res = await ctx.Articles.AddAsync(newArticle);
                await ctx.SaveChangesAsync();
                return _mapper.Map<ArticleDto>(res.Entity);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ArticleDto>> DeleteArticle(Guid id)
        {
            using (var ctx = new DemoContext(_options))
            {
                var dbArticle = ctx.Articles.FirstOrDefault(a => a.Id == id);
                if (dbArticle == null)
                    return BadRequest($"Invalid {nameof(id)}");
                var res = ctx.Articles.Remove(dbArticle);
                await ctx.SaveChangesAsync();
                return _mapper.Map<ArticleDto>(res.Entity);
            }
        }
    }
}