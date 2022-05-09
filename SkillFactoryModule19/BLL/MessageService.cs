using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.BLL.Validators;
using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories.Messages;
using SkillFactoryModule19.DAL.Repositories.Users;
using SkillFactoryModule19.Util;

namespace SkillFactoryModule19.BLL;

public class MessageService
{
    private readonly IValidator<Message> _validator;
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public MessageService(IValidator<Message> validator, IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
    {
        _validator = validator;
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<Unit, IReadOnlyCollection<string>>> Send(Message message)
    {
        var result = _validator.Validate(message);
        
        // var foundUser = await _userRepository.FindByEmail(message.RecipientEMail);
        // if (foundUser is null)
        // {
        //     return new OperationResult<Unit, IReadOnlyCollection<string>>(
        //         new ReadOnlyCollection<string>(new List<string> {$"There is no user with EMail {message.RecipientEMail}"}));
        // }

        if (result.IsError)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(result.GetErrors());
        }
        
        var messageEntity = _mapper.Map<MessageEntity>(message);
        await _messageRepository.Create(messageEntity);

        return new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);
    }
}