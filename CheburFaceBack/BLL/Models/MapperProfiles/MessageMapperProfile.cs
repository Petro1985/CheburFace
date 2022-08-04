using AutoMapper;
using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.BLL.Models.MapperProfiles;

public class MessageMapperProfile : Profile
{
    public MessageMapperProfile()
    {
        CreateMap<MessageEntity, Message>()
            .ForPath(
                entity => entity.Content, 
                expression => expression.
                    MapFrom(message => message.Content))
            .ForPath(
                entity => entity.Id, 
                expression => expression.
                    MapFrom(message => message.Id))
            .ForPath(
                entity => entity.RecipientId, 
                expression => expression.
                    MapFrom(message => message.RecipientId))
            .ForPath(
                entity => entity.SenderId, 
                expression => expression.
                    MapFrom(message => message.SenderId))
            .ReverseMap();
    }
}