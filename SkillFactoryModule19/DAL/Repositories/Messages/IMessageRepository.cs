using Dapper;
using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.DAL.Repositories.Messages;

public interface IMessageRepository
{
    Task Create(MessageEntity messageEntity);
    Task<ICollection<MessageEntity>> FindBySenderId(int senderId);
    Task<ICollection<MessageEntity>> FindByRecipientId(int recipientId);
    Task DeleteById(int messageId);
}

class MessageRepository : BaseSQLiteDapperRepository , IMessageRepository

{
    public MessageRepository(ISqLiteConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task Create(MessageEntity messageEntity)
    {
        var connection = CreateConnection();
        await connection.ExecuteAsync(@"insert into Messages (content, SenderId, RecipientId) 
                             values(:content,:sender_id,:recipient_id)", messageEntity);
    }

    public async Task<ICollection<MessageEntity>> FindBySenderId(int senderId)
    {
        var connection = CreateConnection();
        return (await connection.QueryAsync<MessageEntity>(
            "select * from messages where SenderId = :sender_id", new { sender_id = senderId }))
            .ToList();
    }

    public async Task<ICollection<MessageEntity>> FindByRecipientId(int recipientId)
    {
        var connection = CreateConnection();
        return (await connection.QueryAsync<MessageEntity>(
            "select * from messages where RecipientId = :recipient_id", new { recipient_id = recipientId }))
            .ToList();
    }

    public async Task DeleteById(int messageId)
    {
        var connection = CreateConnection();
        await connection.ExecuteAsync("delete from messages where id = :id", new { id = messageId });
    }
}