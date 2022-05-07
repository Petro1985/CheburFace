using System.Data;
using Dapper;

namespace SkillFactoryModule19.DAL.Repositories;

public abstract class BaseSQLiteDapperRepository<TEntity> : IRepository<TEntity>
{
    private ISqLiteConnectionFactory _connectionFactory;

    protected BaseSQLiteDapperRepository(ISqLiteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public abstract Task Add(TEntity user);

    public abstract Task<TEntity> Get(string email);

    public abstract Task<IEnumerable<TEntity>> GetAll();

    public abstract void Remove(TEntity user);

    public async Task ExecuteRawSql(string sqlScript)
    {
        var connection = CreateConnection();
        connection.Open();
        await connection.ExecuteAsync(sqlScript);
    }

    protected IDbConnection CreateConnection()
    {
        return _connectionFactory.GetOpenedConnection();
    }
}