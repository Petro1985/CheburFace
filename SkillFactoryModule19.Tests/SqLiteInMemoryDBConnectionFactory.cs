using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Dapper;
using SkillFactoryModule19.DAL.Repositories;

namespace SkillFactoryModule19.Tests;

public class SqLiteInMemoryDBConnectionFactory : ISqLiteConnectionFactory
{
    private readonly Lazy<IDbConnection> _connection = new (CreateConnection);

    private static IDbConnection CreateConnection()
    {
        var connection = new SQLiteConnection("Data Source = :memory:; Version = 3; New = True;");
        connection.Open();

        var sql = File.ReadAllText(Path.Combine("SQL","TableUsersCreate.sql"));
        sql += File.ReadAllText(Path.Combine("SQL","TableFriendsCreate.sql"));
        sql += File.ReadAllText(Path.Combine("SQL","TableMessageCreate.sql"));
        connection.Execute(sql);

        return new NotClosableConnectionDecorator(connection);
    }
    public IDbConnection GetOpenedConnection()
    {
        return _connection.Value;
    }
}