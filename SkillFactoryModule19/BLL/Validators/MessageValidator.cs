using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.Util;

namespace SkillFactoryModule19.BLL.Validators;

public class MessageValidator : IValidator<Message>
{
    public ValidationResult Validate(Message entity)
    {
        return ValidationResult.FromCheck(!String.IsNullOrWhiteSpace(entity.Content), "Message can't be empty")
            .Check((entity.Content?.Length ?? 0) < 5000, "Message can't be longer than 5000 symbols");
    }
}