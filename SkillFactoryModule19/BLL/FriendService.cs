using AutoMapper;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.BLL.Validators;
using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories.Friends;
using SkillFactoryModule19.Util;

namespace SkillFactoryModule19.BLL;

public class FriendService
{
    private readonly IFriendRepository _friendRepository;
    private readonly IMapper _mapper;

    public FriendService(IFriendRepository messageRepository, IMapper mapper)
    {
        _friendRepository = messageRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<Unit, string>> AddFriend(Friend friend)
    {
        var friendEntity = _mapper.Map<FriendEntity>(friend);

        try
        {
            await _friendRepository.Create(friendEntity);
        }
        catch (Exception e)
        {
            return new OperationResult<Unit, string>($"Didn't manage to add friend");
        }
        
        return new OperationResult<Unit, string>(Unit.Instance);
    }

    public async Task<OperationResult<Unit, string>> ObliterateFriend(Friend friend)
    {
        var friendEntity = _mapper.Map<FriendEntity>(friend);

        try
        {
            await _friendRepository.Delete(friendEntity);
        }
        catch (Exception e)
        {
            return new OperationResult<Unit, string>($"Didn't manage to obliterate friend");
        }
        
        return new OperationResult<Unit, string>(Unit.Instance);
    }

    public async Task<ICollection<Friend>> GetFriends(int userId)
    {
        var friends = await _friendRepository.FindByUserId(userId);
        
        return friends.Select(_mapper.Map<Friend>).ToList();
    }
}