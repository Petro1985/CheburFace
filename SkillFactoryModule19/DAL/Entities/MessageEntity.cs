namespace SkillFactoryModule19.DAL.Entities;

public class MessageEntity
{
    public MessageEntity(int id, string content, int senderId, int recipientId)
    {
        Id = id;
        Content = content;
        SenderId = senderId;
        RecipientId = recipientId;
    }

    public int Id { get; set; }
    public string Content { get; set; }
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
}