using AutoMapper;
using DotnetApiDemo.Domain;
using DotnetApiDemo.Dto;

namespace DotnetApiDemo.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
                )
                .ReverseMap()
                .ForMember(src => src.FirstName,
                opt => opt.MapFrom(dest => GetNamePart(dest, 0)))
                .ForMember(src => src.LastName,
                opt => opt.MapFrom(dest => GetNamePart(dest, 1)));

        }

        private static string GetNamePart(UserDto dest, int index)
        {
            return dest.Name.Split(" ")[index];
        }
    }
}
