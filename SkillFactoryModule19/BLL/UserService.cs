using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories;
using SkillFactoryModule19.DAL.Repositories.Users;
using SkillFactoryModule19.Util;

namespace SkillFactoryModule19.BLL;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserEntity> _validator;

    public UserService(IUserRepository userRepository, IValidator<UserEntity> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<OperationResult<Unit, IReadOnlyCollection<string>>> AddUser(UserEntity user)
    {
        var result = _validator.Validate(user);
        if (result.IsError)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(result.GetErrors());
        }

        var isUnique = (await _userRepository.FindByEmail(user.EMail)) is null;
        if (!isUnique)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(new List<string> {"User with this EMail is already registered"});
        } 
        
        await _userRepository.Create(user);
        return new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);
    }

    public async Task<UserEntity> GetUser(string email)
    {
        return await _userRepository.FindByEmail(email);
    }
    
    public void ObliterateUser(UserEntity user)
    {
        
    }
}