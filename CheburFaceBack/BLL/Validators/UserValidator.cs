using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.Util;

namespace SkillFactoryModule19.BLL.Validators;

public class UserValidator : IValidator<User>
{
    private ValidationResult ValidateEMail(string? email)
    {
        const string pattern = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";
        var result = ValidationResult.FromCheck(!String.IsNullOrWhiteSpace(email), "EMail can't be empty");
        bool regexSuccess = Regex.Match(email??"", pattern, RegexOptions.Compiled).Success;
        
        return result.Check(regexSuccess, "Incorrect EMail");
    }
    
    private ValidationResult ValidatePassword(string? password)
    {
        var result = ValidationResult
            .FromCheck(
                (password?.Length ?? 0) > 7
                , "Password length is less than 8 symbols")
            .Check(
                password?.Any(Char.IsDigit)
                , "Password must contain at least one digit")
            .Check(
                password is not null && password.Any(Char.IsLower) && password.Any(Char.IsUpper)
                , "Password must contain lower and upper letters");
        
        
        return result;
    }

    private ValidationResult ValidateName(string? name, string propertyName)
        => ValidationResult
            .FromCheck(!String.IsNullOrWhiteSpace(name),
                propertyName + " must be not empty");
    
    public ValidationResult Validate(User user)
        => ValidatePassword(user.Password)
            .CombineWith(ValidateName(user.FirstName, "First name"))
            .CombineWith(ValidateName(user.LastName, "Last name"))
            .CombineWith(ValidateEMail(user.EMail));
    
}