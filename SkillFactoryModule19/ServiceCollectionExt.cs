using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.BLL.Models.MapperProfiles;
using SkillFactoryModule19.BLL.Validators;
using SkillFactoryModule19.DAL.Repositories;
using SkillFactoryModule19.DAL.Repositories.Friends;
using SkillFactoryModule19.DAL.Repositories.Messages;
using SkillFactoryModule19.DAL.Repositories.Users;

public static class ServiceCollectionExt
{
    public static IServiceCollection AddCheburFace(this IServiceCollection collection)
    {
        collection.AddSingleton<UserService>();
        collection.AddSingleton<MessageService>();
        collection.AddSingleton<FriendService>();
        collection.AddSingleton<IUserRepository, SqLiteDapperRepositoryUser>();
        collection.AddSingleton<IPhotoRepository, PhotoRepository>();
        collection.AddSingleton<IMessageRepository, SqLiteDapperRepositoryMessage>();
        collection.AddSingleton<IFriendRepository, SqLiteDapperRepositoryFriend>();
        collection.AddSingleton<IValidator<User>, UserValidator>();
        collection.AddSingleton<IValidator<Message>, MessageValidator>();
        collection.AddSingleton<ISqLiteConnectionFactory, SqLiteFileDBConnectionFactory>(
            _ =>
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dataBase.db");
                return new SqLiteFileDBConnectionFactory($"Data Source={path};Version=3;");
            });

        var assembly = Assembly.GetAssembly(typeof(UserMapperProfile));
        collection.AddAutoMapper(assembly);

        return collection;
    }
}