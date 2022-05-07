using System.Data;
using System.Data.SQLite;

namespace SkillFactoryModule19.DAL.Repositories;

public class SqLiteFileDBConnectionFactory : ISqLiteConnectionFactory
{
    private readonly string _connectionString;
    public SqLiteFileDBConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetOpenedConnection()
    {
        var connection = new SQLiteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}