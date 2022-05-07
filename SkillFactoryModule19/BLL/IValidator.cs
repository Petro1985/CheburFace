using SkillFactoryModule19.Util;

namespace SkillFactoryModule19.BLL;

public interface IValidator<T>
{
    public ValidationResult Validate(T entity);
}