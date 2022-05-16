namespace SkillFactoryModule19.DAL.Entities;

public class MessageEntity
{
    private MessageEntity()
    {
    }

    public MessageEntity(string content, int senderId, int recipientId)
    {
        Content = content;
        SenderId = senderId;
        RecipientId = recipientId;
    }

    public int Id { get; private init; }
    public string Content { get; init; }
    public int SenderId { get; init; }
    public int RecipientId { get; init; }
}