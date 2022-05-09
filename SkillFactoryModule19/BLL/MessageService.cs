using System.ComponentModel.DataAnnotations;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.DAL.Repositories.Messages;
using SkillFactoryModule19.Util;

namespace SkillFactoryModule19.BLL;

public class MessageService
{
    private IValidator<Message> _validator;
    private IMessageRepository _messageRepository;

    public OperationResult<Unit, string> Create(Message message)
    {
        return new OperationResult<Unit, string>(Unit.Instance);
    }
}