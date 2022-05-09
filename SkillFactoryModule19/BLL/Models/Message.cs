namespace SkillFactoryModule19.BLL.Models;

public class Message
{
    public Message(int id, string content, int senderId, int recipientId)
    {
        Id = id;
        Content = content;
        SenderId = senderId;
        RecipientId = recipientId;
    }

    public int Id { get; private init; }
    public string Content { get; set; }
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
}