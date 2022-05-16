namespace SkillFactoryModule19.BLL.Models;

public class Friend
{
    private Friend()
    {
    }

    public Friend(int userId, int friendId)
    {
        UserId = userId;
        FriendId = friendId;
    }

    public int Id { get; set; }
    public int UserId { get; set; }    
    public int FriendId { get; set; }    
    
}