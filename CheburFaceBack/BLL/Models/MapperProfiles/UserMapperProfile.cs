using AutoMapper;
using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.BLL.Models.MapperProfiles;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserEntity>()
            .ForMember(item => item.Id, option => option.MapFrom(user=> user.Id))
            .ReverseMap();
    }
}