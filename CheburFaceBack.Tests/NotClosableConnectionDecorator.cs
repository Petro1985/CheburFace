using System.Data;

namespace SkillFactoryModule19.Tests;

public class NotClosableConnectionDecorator : IDbConnection
{
    private readonly IDbConnection _decoratee;

    public NotClosableConnectionDecorator(IDbConnection decoratee)
    {
        _decoratee = decoratee;
    }

    public void Dispose() {}

    public IDbTransaction BeginTransaction()
    {
        return _decoratee.BeginTransaction();
    }

    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
        return _decoratee.BeginTransaction(il);
    }

    public void ChangeDatabase(string databaseName)
    {
        _decoratee.ChangeDatabase(databaseName);
    }

    public void Close()
    {

    }

    public IDbCommand CreateCommand()
    {
        return _decoratee.CreateCommand();
    }

    public void Open()
    {

    }

    public string ConnectionString
    {
        get => _decoratee.ConnectionString;
        set => _decoratee.ConnectionString = value;
    }

    public int ConnectionTimeout => _decoratee.ConnectionTimeout;

    public string Database => _decoratee.Database;

    public ConnectionState State => _decoratee.State;
}