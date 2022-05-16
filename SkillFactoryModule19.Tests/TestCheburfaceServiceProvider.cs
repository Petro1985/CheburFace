using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.BLL.Models.MapperProfiles;
using SkillFactoryModule19.BLL.Validators;
using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories;
using SkillFactoryModule19.DAL.Repositories.Friends;
using SkillFactoryModule19.DAL.Repositories.Messages;
using SkillFactoryModule19.DAL.Repositories.Users;

namespace SkillFactoryModule19.Tests;

public class TestCheburfaceServiceProvider : IServiceProvider
{
    private readonly IServiceCollection _collection = new ServiceCollection();
    private readonly IServiceProvider _provider;

    public TestCheburfaceServiceProvider()
    {
        _collection.AddCheburFace();
        _collection.Replace(ServiceDescriptor.Singleton<ISqLiteConnectionFactory, SqLiteInMemoryDbConnectionFactory>());

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