using AutoMapper;
using DotnetApiDemo.Domain;
using DotnetApiDemo.Dto;

namespace DotnetApiDemo.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {

            CreateMap<Article, ArticleDto>()
                .ReverseMap();
        }
    }
}
