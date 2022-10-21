using AutoMapper;
using DotnetApiDemo.Domain;
using DotnetApiDemo.Dto;
using Microsoft.EntityFrameworkCore;

namespace DotnetApiDemo.Services
{
    public class ArticleService
    {
        private readonly DbContextOptions<DemoContext> _options;
        private readonly IMapper _mapper;
        public ArticleService(DbContextOptions<DemoContext> options, IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync(FilterDto filter)
        {
            using (var ctx = new DemoContext(_options))
            {
                var hasNameFilter = !string.IsNullOrWhiteSpace(filter.Name);
                return await ctx.Articles.Where(a =>
                    (hasNameFilter ? a.Name.Contains(filter.Name) : true) &&
                    (!filter.PriceLessThan.HasValue || a.Price < filter.PriceLessThan) &&
                    (!filter.PriceGreaterThan.HasValue || a.Price > filter.PriceGreaterThan))
                    .ToListAsync();

            }
        }

        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
            using (var ctx = new DemoContext(_options))
            {
                return await ctx.Articles.FirstOrDefaultAsync(a => a.Id == id);
            }
        }

        public async Task<ArticleDto> UpdateArticleAsync(Guid id, ArticleDto articleDto)
        {
            if (id == Guid.Empty || articleDto.Id == Guid.Empty)
                throw new BadHttpRequestException($"Invalid {nameof(articleDto.Id)}");
            using (var ctx = new DemoContext(_options))
            {
                var dbArticle = ctx.Articles.FirstOrDefault(a => a.Id == id);
                if (dbArticle == null)
                    throw new BadHttpRequestException($"Invalid {nameof(articleDto.Id)}");
                dbArticle.Name = articleDto.Name;
                dbArticle.Price = articleDto.Price;
                dbArticle.Unit = articleDto.Unit;

                var res = ctx.Update(dbArticle);
                await ctx.SaveChangesAsync(); //fontos különben nem jutnak érvényre a változtatások
                var updatedArticle = _mapper.Map<ArticleDto>(res.Entity);
                return updatedArticle;
            }
        }

        public async Task<ArticleDto> CreateArticleAsync(ArticleDto articleDto)
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

        public async Task<ArticleDto> DeleteArticleAsync(Guid id)
        {
            using (var ctx = new DemoContext(_options))
            {
                var dbArticle = ctx.Articles.FirstOrDefault(a => a.Id == id);
                if (dbArticle == null)
                    throw new BadHttpRequestException($"Invalid {nameof(id)}");
                var res = ctx.Articles.Remove(dbArticle);
                await ctx.SaveChangesAsync();
                return _mapper.Map<ArticleDto>(res.Entity);
            }
        }
    }
}
