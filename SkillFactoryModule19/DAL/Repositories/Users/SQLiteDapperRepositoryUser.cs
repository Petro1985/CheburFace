using Dapper;
using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.DAL.Repositories.Users;

public class SqLiteDapperRepositoryUser : BaseSQLiteDapperRepository, IUserRepository
{
    public async Task Create(UserEntity user)
    {
        using var dbConnection = CreateConnection();

        string sqlQuery = @"insert into Users (FirstName, LastName, Password, EMail, Photo, FavoriteMovie, FavoriteBook)
            VALUES (:FirstName, :LastName, :Password, :EMail, :Photo, :FavoriteMovie, :FavoriteBook);";

        await dbConnection.ExecuteAsync(sqlQuery, user);
    }
    public async Task<ICollection<UserEntity>> FindAll()
    {
        using var dbConnection = CreateConnection();
        string sqlQuery = @"select * from Users";
        var result = await dbConnection.QueryAsync<UserEntity>(sqlQuery);
        
        return result.ToArray();
    }

    public async Task<UserEntity> FindByEmail(string email)
    {
        using var dbConnection = CreateConnection();
        string sqlQuery = @"select * from Users where EMail = :UserEmail";
        var result = await dbConnection.QueryFirstOrDefaultAsync<UserEntity>(sqlQuery, new {UserEMail = email});
        
        return result;
    }

    public async Task<UserEntity> FindById(int id)
    {
        using var dbConnection = CreateConnection();
        string sqlQuery = @"select * from Users where Id = :UserId";
        var result = await dbConnection.QueryFirstOrDefaultAsync<UserEntity>(sqlQuery, new {UserId = id});
        
        return result;
    }

    public async Task Update(UserEntity user)
    {
        using var dbConnection = CreateConnection();

        string sqlQuery =
            @"update users set Firstname = :Firstname, Lastname = :Lastname, Password = :Password, email = :Email, photo = :Photo, FavoriteMovie = :FavoriteMovie, FavoriteBook = :FavoriteBook where id = :Id";
        await dbConnection.ExecuteAsync(sqlQuery, user);
    }

    public async Task DeleteById(int id)
    {
        using var dbConnection = CreateConnection();
        string sqlQuery =
            @"delete from users where id = :Id";
        await dbConnection.ExecuteAsync(sqlQuery, new { Id = id });
    }

    public SqLiteDapperRepositoryUser(ISqLiteConnectionFactory connectionFactory) : base(connectionFactory)
    {
        
    }
}