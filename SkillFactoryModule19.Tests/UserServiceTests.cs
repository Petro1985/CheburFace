using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.DAL.Entities;
using SkillFactoryModule19.DAL.Repositories;
using SkillFactoryModule19.DAL.Repositories.Users;
using SkillFactoryModule19.Util;
using Xunit;
using ValidationResult = SkillFactoryModule19.Util.ValidationResult;

namespace SkillFactoryModule19.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task FailValidationTest()
    {
        SqLiteDapperRepositoryUser userRepository = new SqLiteDapperRepositoryUser(new SqLiteInMemoryDBConnectionFactory());
        var userService = new UserService(
            userRepository
            , new UserValidation());

        var user = new UserEntity() {
            Id = 1,
            FirstName = "", LastName = null,
            Password = "sdsd",
            EMail = "dsdfsdf",
            Photo = "dasd",
            FavoriteBook = "sadasd",
            FavoriteMovie = "asdasd"
        };
        
        var expectedResult = new OperationResult<Unit, IReadOnlyCollection<string>>(
            new string[]
            {
                "Password length is less than 8 symbols",
                "Password must contain at least one digit",
                "Password must contain lower and upper letters",
                "First name must be not empty",
                "Last name must be not empty",
                "Incorrect EMail",
            });

        var addResult = await userService.AddUser(user);
        
        addResult.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact]
    public async Task SuccessValidationTest()
    {
        SqLiteDapperRepositoryUser userRepository = new SqLiteDapperRepositoryUser(new SqLiteInMemoryDBConnectionFactory());
        var userService = new UserService(
            userRepository
            , new UserValidation());

        var user = new UserEntity() {
            Id = 1,
            FirstName = "Qrwerwer", LastName = "SDFwerwe",
            Password = "sdsdwQWEqe13",
            EMail = "MyEmail@mail.ru",
            Photo = "dasd",
            FavoriteBook = "sadasd",
            FavoriteMovie = "asdasd"
        };
        
        var expectedResult = new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);

        var addResult = await userService.AddUser(user);
        addResult.Should().BeEquivalentTo(expectedResult);

    }
    
    [Fact]
    public async Task AddAndGetUserTest()
    {
        SqLiteDapperRepositoryUser userRepository = new SqLiteDapperRepositoryUser(new SqLiteInMemoryDBConnectionFactory());
        var userService = new UserService(
            userRepository
            , new UserValidation());

        var user = new UserEntity() {
            Id = 15,
            FirstName = "TestUserFN", LastName = "TestUserLN",
            Password = "qwe123QWE!@#",
            EMail = "MyEmail@mail.ru",
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
        SqLiteDapperRepositoryUser userRepository = new SqLiteDapperRepositoryUser(new SqLiteInMemoryDBConnectionFactory());
        var userService = new UserService(
            userRepository
            , new UserValidation());

        var user = new UserEntity() {
            Id = 15,
            FirstName = "TestUserFN", LastName = "TestUserLN",
            Password = "qwe123QWE!@#",
            EMail = "MyEmail@mail.ru",
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        
        var addResult = await userService.AddUser(user);
        var secondAddResult = await userService.AddUser(user);

        secondAddResult.IsSuccessful.Should().BeFalse();
    }
    
    [Fact]
    public async Task AddMockedTest()
    {
        var mockedRepository = Substitute.For<IUserRepository>();
        var mockedValidator = Substitute.For<IValidator<UserEntity>>();
        mockedValidator.Validate(Arg.Any<UserEntity>()).Returns(new ValidationResult());
        
        var userService = new UserService(
            mockedRepository
            , mockedValidator);

        var user = new UserEntity() {
            Id = 15,
            FirstName = "TestUserFN", LastName = "TestUserLN",
            Password = "qwe123QWE!@#",
            EMail = "MyEmail@mail.ru",
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        
        var addResult = await userService.AddUser(user);
        
        await mockedRepository.Received(1).Create(Arg.Any<UserEntity>());
        mockedValidator.Received(1).Validate(Arg.Any<UserEntity>());
        
        addResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public async Task AddDuplicatedEMailMockedTest()
    {
        var mockedRepository = Substitute.For<IUserRepository>();
        var mockedValidator = Substitute.For<IValidator<UserEntity>>();
        mockedValidator.Validate(Arg.Any<UserEntity>()).Returns(new ValidationResult());
        
        var userService = new UserService(
            mockedRepository
            , mockedValidator);
        
        var user = new UserEntity() {
            Id = 15,
            FirstName = "TestUserFN", LastName = "TestUserLN",
            Password = "qwe123QWE!@#",
            EMail = "MyEmail@mail.ru",
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        mockedRepository.FindByEmail("MyEmail@mail.ru").Returns(user);

        var addResult = await userService.AddUser(user);

        mockedRepository.ReceivedCalls().Should().OnlyContain(p => p.GetMethodInfo().Name == nameof(IUserRepository.FindByEmail));
        addResult.IsSuccessful.Should().BeFalse();
    }
}