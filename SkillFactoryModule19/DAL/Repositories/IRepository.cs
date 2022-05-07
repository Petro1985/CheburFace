namespace SkillFactoryModule19.DAL.Repositories;

public interface IRepository<T> 
{
    public Task Add(T user);
    public Task<T> Get(string email);
    public Task<IEnumerable<T>> GetAll();
    public void Remove(T user);
}