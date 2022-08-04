using SkillFactoryModule19.DAL.Entities;

namespace SkillFactoryModule19.DAL.Repositories.Friends;

public interface IFriendRepository
{
        public Task Create(FriendEntity friend);
        public Task<ICollection<FriendEntity>> FindByUserId(int id);
        public Task Delete(FriendEntity friend);
}