using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.BLL.Validators;
using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories.Users;
using Xunit;
using ValidationResult = SkillFactoryModule19.Util.ValidationResult;

namespace SkillFactoryModule19.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task AddAndGetUserTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User("TestUserFN", "TestUserLN","qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        
        var addResult = await userService.AddUser(user);
        addResult.IsSuccessful.Should().BeTrue();
        var getResult = await userService.GetUser("MyEmail@mail.ru");

        getResult.Should().BeEquivalentTo(user, options => options.Excluding(qwe => qwe.Id));
    }

    [Fact]
    public async Task AddExistedEMailTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User ("TestUserFN", "TestUserLN","qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        
        var addResult = await userService.AddUser(user);
        var secondAddResult = await userService.AddUser(user);

        secondAddResult.IsSuccessful.Should().BeFalse();
    }

    [Fact]
    public async Task SuccessfulUpdateUserTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User ("TestUserFN", "TestUserLN","qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };

        await userService.AddUser(user);
        var user2 = await userService.GetUser("MyEmail@mail.ru");
        user2!.FirstName = "New Test Name";

        var result = await userService.UpdateUser(user2);
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public async Task FailUpdateUserTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User ("TestUserFN", "TestUserLN","qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        
        var user2 = new User ("TestUserFN", "TestUserLN","qwe123QWE!@#", "MyTestEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };

        await userService.AddUser(user);
        await userService.AddUser(user2);
        
        var user3 = await userService.GetUser("MyEmail@mail.ru");
        
        user3!.EMail = "MyTestEmail@mail.ru";
        var result = await userService.UpdateUser(user3);
        
        result.IsSuccessful.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Count.Should().Be(1);
    }

    [Fact]
    public async Task SuccessfulAuthorisationTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User ("TestUserFN", "TestUserLN","qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        var usersAuthenticationData = new UserAuthenticationData()
        {
            Email = "MyEmail@mail.ru",
            Password = "qwe123QWE!@#",
        };
        
        await userService.AddUser(user);
        
        var result = await userService.AuthenticateUser(usersAuthenticationData);
        
        result.IsSuccessful.Should().BeTrue();
        result.Result.Should().BeEquivalentTo(user, options => options.Excluding(item => item.Id));
    } 
        
    [Fact]
    public async Task IncorrectPasswordAuthorisationTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User ("TestUserFN", "TestUserLN","qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        var usersAuthenticationData = new UserAuthenticationData()
        {
            Email = "MyEmail@mail.ru",
            Password = "qwe12366QWE!@#",
        };
        
        await userService.AddUser(user);
        
        var result = await userService.AuthenticateUser(usersAuthenticationData);
        
        result.IsSuccessful.Should().BeFalse();
        result.Error.Should().NotBeNull();
    } 

    [Fact]
    public async Task IncorrectEMailAuthorisationTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User ("TestUserFN", "TestUserLN","qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        var usersAuthenticationData = new UserAuthenticationData()
        {
            Email = "MyEmai44l@mail.ru",
            Password = "qwe123QWE!@#",
        };
        
        await userService.AddUser(user);
        
        var result = await userService.AuthenticateUser(usersAuthenticationData);
        
        result.IsSuccessful.Should().BeFalse();
    } 
    
    [Fact]
    public async Task AddMockedTest()
    {
        var mockedRepository = Substitute.For<IUserRepository>();
        var mockedValidator = Substitute.For<IValidator<User>>();
        mockedValidator.Validate(Arg.Any<User>()).Returns(new ValidationResult());
        
        var provider = new TestCheburfaceServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();
        
        var userService = new UserService(
            mockedRepository
            , mockedValidator, mapper, new PhotoRepository());

        var user = new User("TestUserFN", "TestUserLN", "qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        
        var addResult = await userService.AddUser(user);
        
        await mockedRepository.Received(1).Create(Arg.Any<UserEntity>());
        mockedValidator.Received(1).Validate(Arg.Any<User>());
        
        addResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task AddDuplicatedEMailMockedTest()
    {
        var mockedRepository = Substitute.For<IUserRepository>();
        var mockedValidator = Substitute.For<IValidator<User>>();
        
        mockedValidator.Validate(Arg.Any<User>()).Returns(new ValidationResult());

        var provider = new TestCheburfaceServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();
        
        var userService = new UserService(
            mockedRepository
            , mockedValidator, mapper, new PhotoRepository());
        
        var user = new User("TestUserFN", "TestUserLN", "qwe123QWE!@#", "MyEmail@mail.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        
        mockedRepository.FindByEmail("MyEmail@mail.ru").Returns(new UserEntity(){EMail = "MyEmail@mail.ru"});

        var addResult = await userService.AddUser(user);

        mockedRepository.ReceivedCalls().Should().OnlyContain(p => p.GetMethodInfo().Name == nameof(IUserRepository.FindByEmail));
        addResult.IsSuccessful.Should().BeFalse();
    }
}