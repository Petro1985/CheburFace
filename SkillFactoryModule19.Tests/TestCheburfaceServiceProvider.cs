using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories;
using SkillFactoryModule19.DAL.Repositories.Users;

namespace SkillFactoryModule19.Tests;

public class TestCheburfaceServiceProvider : IServiceProvider
{
    private IServiceCollection _collection = new ServiceCollection();
    private IServiceProvider _provider;


    public TestCheburfaceServiceProvider()
    {
        _collection.AddSingleton<UserService>();
        _collection.AddSingleton<IUserRepository, SqLiteDapperRepositoryUser>();
        _collection.AddSingleton<IValidator<User>, UserValidator>();
        _collection.AddSingleton<ISqLiteConnectionFactory, SqLiteInMemoryDBConnectionFactory>();

        var assembly = Assembly.GetAssembly(typeof(UserMapperProfile));
        _collection.AddAutoMapper(assembly);

        _provider = _collection.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true,
        } );
    }

    public object? GetService(Type serviceType)
    {
        return _provider.GetService(serviceType);
    }
}