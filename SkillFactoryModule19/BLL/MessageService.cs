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

        if (result.IsError)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(result.GetErrors());
        }
        
        var messageEntity = _mapper.Map<MessageEntity>(message);

        try
        {
            await _messageRepository.Create(messageEntity);
        }
        catch (Exception e)
        {
            return new OperationResult<Unit, IReadOnlyCollection<string>>(new List<string>
                {"Didn't manage to create new message"});
        }
        return new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);
    }

    public async Task<ICollection<Message>> GetMessagesBySender(int senderId)
    {
        var messages = await _messageRepository.FindByRecipientId(senderId);

        return messages
            .Select(_mapper.Map<Message>)
            .ToList();
    }
    public async Task<ICollection<Message>> GetMessagesByRecipient(int recipientId)
    {
        var messages = await _messageRepository.FindByRecipientId(recipientId);

        return messages
            .Select(_mapper.Map<Message>)
            .ToList();
    }
    
    public async Task<ICollection<Message>> GetMessages(int userId)
    {
        var messages = await _messageRepository.FindAllMessages(userId);

        return messages
            .Select(_mapper.Map<Message>)
            .ToList();
    }

    public async Task ObliterateMessage(int id)
    {
        await _messageRepository.DeleteById(id);
    }
    
}