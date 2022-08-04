namespace SkillFactoryModule19.BLL.Models;

public class Message
{
    public Message(string content, int senderId, int recipientId)
    {
        Content = content;
        SenderId = senderId;
        RecipientId = recipientId;
    }

    public int Id { get; private init; }
    public string Content { get; set; }
    public int SenderId { get; set; }
    public int RecipientId { get; set; }
}