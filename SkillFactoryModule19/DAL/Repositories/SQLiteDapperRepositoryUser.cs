using Dapper;
using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.DAL.Repositories;

public class SqLiteDapperRepositoryUser : BaseSQLiteDapperRepository<UserEntity>
{
    public override async Task Add(UserEntity user)
    {
        using var dbConnection = CreateConnection();

        string sqlQuery = @"insert into Users (FirstName, LastName, Password, EMail, Photo, FavoriteMovie, FavoriteBook)
            VALUES (@FirstName, @LastName, @Password, @EMail, @Photo, @FavoriteMovie, @FavoriteBook);";

        await dbConnection.ExecuteAsync(sqlQuery, user);
    }

    public override async Task<UserEntity> Get(string email)
    {
        using var dbConnection = CreateConnection();
        string sqlQuery = @"select * from Users where EMail = @UserEmail";
        var result = await dbConnection.QueryFirstOrDefaultAsync<UserEntity>(sqlQuery, new {UserEMail = email});
        
        return result;
    }

    public override Task<IEnumerable<UserEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public override void Remove(UserEntity user)
    {
        throw new NotImplementedException();
    }

    public SqLiteDapperRepositoryUser(ISqLiteConnectionFactory connectionFactory) : base(connectionFactory)
    {
        
    }
}