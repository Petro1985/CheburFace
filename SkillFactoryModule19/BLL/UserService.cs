using System.Collections.ObjectModel;
using AutoMapper;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories;
using SkillFactoryModule19.DAL.Repositories.Users;
using SkillFactoryModule19.Util;

namespace SkillFactoryModule19.BLL;

public class UserService
{
    private IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<User> _validator;

    public UserService(IUserRepository userRepository, IValidator<User> validator, IMapper mapper)
    {
        _userRepository = userRepository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<OperationResult<Unit, IReadOnlyCollection<string>>> AddUser(User user)
    {
        var result = _validator.Validate(user);
        var userEntity = _mapper.Map<UserEntity>(user);
        
        if (result.IsError)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(result.GetErrors());
        }

        var isUnique = (await _userRepository.FindByEmail(userEntity.EMail)) is null;
        if (!isUnique)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(new List<string> {"User with this EMail is already registered"});
        } 
        
        await _userRepository.Create(userEntity);
        return new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);
    }

    public async Task<UserEntity> GetUser(string email)
    {
        return await _userRepository.FindByEmail(email);
    }

    public async Task<OperationResult<Unit, IReadOnlyCollection<string>>> UpdateUser(User user)
    {
        var result = _validator.Validate(user);
        if (result.IsError)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(result.GetErrors());
        }
        
        var entityInDb = (await _userRepository.FindByEmail(user.EMail));
        
        if (entityInDb is not null && entityInDb.Id != user.Id)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(new List<string> {"User with this EMail is already registered"});
        }

        var userEntity = _mapper.Map<UserEntity>(user);
        await _userRepository.Update(userEntity);
        
        return new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);
    }
    public void ObliterateUser(UserEntity user)
    {
        
    }
}