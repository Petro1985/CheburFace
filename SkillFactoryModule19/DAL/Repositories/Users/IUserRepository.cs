using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.DAL.Repositories.Users;

public interface IUserRepository
{
    public Task Create(UserEntity user);
    public Task<UserEntity?> FindByEmail(string email);
    public Task<ICollection<UserEntity>?> FindAll();
    public Task<UserEntity?> FindById(int id);
    public Task Update(UserEntity user);
    public Task DeleteById(int id);
}