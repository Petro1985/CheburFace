using System.Data;
using Dapper;
using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.DAL.Repositories;

public abstract class BaseSQLiteDapperRepository
{
    private readonly ISqLiteConnectionFactory _connectionFactory;

    protected BaseSQLiteDapperRepository(ISqLiteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    protected IDbConnection CreateConnection()
    {
        return _connectionFactory.GetOpenedConnection();
    }
}