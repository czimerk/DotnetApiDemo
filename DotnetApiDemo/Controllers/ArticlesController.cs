using AutoMapper;
using DotnetApiDemo;
using DotnetApiDemo.Domain;
using DotnetApiDemo.Dto;
using DotnetApiDemo.Services;
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
        private readonly IArticleService _articleService;
        private readonly DbContextOptions<DemoContext> _options;

        public ArticlesController(ILogger<ArticlesController> logger, DbContextOptions<DemoContext> options, IMapper mapper, IArticleService articleService)
        {
            _logger = logger;
            _options = options;
            _mapper = mapper;
            _articleService = articleService;
        }

        //Nem változtatja a Route-ot
        [HttpGet(Name = "GetArticles")]
        public async Task<IEnumerable<Article>> Get([FromQuery] FilterDto filter)
        {
            return await _articleService.GetArticlesAsync(filter);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Article> GetById(Guid id)
        {
            return await _articleService.GetArticleByIdAsync(id);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ArticleDto>> UpdateArticle(Guid id, [FromBody] ArticleDto articleDto)
        {
            return await _articleService.UpdateArticleAsync(id, articleDto);
        }

        [HttpPost]
        public async Task<ArticleDto> AddArticle(ArticleDto articleDto) // nem kell [FromBody] mert complex típus
        {
            return await _articleService.CreateArticleAsync(articleDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ArticleDto>> DeleteArticle(Guid id)
        {
            return await _articleService.DeleteArticleAsync(id);
        }

       
    }
}