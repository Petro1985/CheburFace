using AutoMapper;
using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.BLL.Models.MapperProfiles;

public class FriendMapperProfile : Profile
{
    public FriendMapperProfile()
    {
        CreateMap<Friend, FriendEntity>()
            .ReverseMap();
    }
}