using DotnetApiDemo.Domain;
using DotnetApiDemo.Dto;

namespace DotnetApiDemo.Services
{
    public interface IArticleService
    {
        Task<ArticleDto> CreateArticleAsync(ArticleDto articleDto);
        Task<ArticleDto> DeleteArticleAsync(Guid id);
        Task<Article> GetArticleByIdAsync(Guid id);
        Task<IEnumerable<Article>> GetArticlesAsync(FilterDto filter);
        Task<ArticleDto> UpdateArticleAsync(Guid id, ArticleDto articleDto);
    }
}