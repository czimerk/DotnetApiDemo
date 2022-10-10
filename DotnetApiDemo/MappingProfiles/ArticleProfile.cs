using AutoMapper;
using DotnetApiDemo.Dto;
using GraphQLDemo.Domain;

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
