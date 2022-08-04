using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.DAL.Repositories.Messages;

public interface IMessageRepository
{
    Task Create(MessageEntity messageEntity);
    Task<ICollection<MessageEntity>> FindBySenderId(int senderId);
    Task<ICollection<MessageEntity>> FindByRecipientId(int recipientId);
    Task<ICollection<MessageEntity>> FindAllMessages(int userId);
    Task DeleteById(int messageId);
}