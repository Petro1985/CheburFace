using Dapper;
using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.DAL.Repositories.Friends;

public class SqLiteDapperRepositoryFriend : BaseSQLiteDapperRepository, IFriendRepository
{
    public async Task Create(FriendEntity friend)
    {
        var connection = CreateConnection();
        await connection.ExecuteAsync(@"insert into friends (UserId, FriendId) values (:UserId, :FriendId)", friend);
    }

    public async Task<ICollection<FriendEntity>> FindByUserId(int id)
    {
        var connection = CreateConnection();
        return (await connection.QueryAsync<FriendEntity>(@"select * from friends where UserId = :user_id",
            new {user_id = id})).ToList();
    }

    public async Task Delete(FriendEntity friend)
    {
        var connection = CreateConnection();
        await connection.ExecuteAsync(@"delete from friends where FriendId = :FriendId and UserId = :UserId", friend);                
    }

    public SqLiteDapperRepositoryFriend(ISqLiteConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }
}