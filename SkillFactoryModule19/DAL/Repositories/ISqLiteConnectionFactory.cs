using System.Data;

namespace SkillFactoryModule19.DAL.Repositories;

public interface ISqLiteConnectionFactory
{
    public IDbConnection GetOpenedConnection();
}