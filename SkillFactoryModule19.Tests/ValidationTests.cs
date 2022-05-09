using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;
using SkillFactoryModule19.Util;
using Xunit;

namespace SkillFactoryModule19.Tests;

public class ValidationTests
{
    [Fact]
    public async Task ShortPasswordTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User("asdasd", "adasdasd", "s1Dsd", "MyEmail@mail.ru")
        {
            Photo = "dasd",
            FavoriteBook = "sadasd",
            FavoriteMovie = "asdasd"
        };

        var result = await userService.AddUser(user);

        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        result.Error!.Count.Should().Be(1);
    }
    
    [Fact]
    public async Task NoDigitsPasswordTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User("asdasd", "adasdasd", "sasdasSdDsd", "MyEmail@mail.ru")
        {
            Photo = "dasd",
            FavoriteBook = "sadasd",
            FavoriteMovie = "asdasd"
        };

        var result = await userService.AddUser(user);

        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        result.Error.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task OnlyLowOrUpperLettersPasswordTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User("asdasd", "adasdasd", "sasda11sdsd", "MyEmail@mail.ru")
        {
            Photo = "dasd",
            FavoriteBook = "sadasd",
            FavoriteMovie = "asdasd"
        };

        var result = await userService.AddUser(user);

        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        result.Error!.Count.Should().Be(1);
    }

    [Fact]
    public async Task SuccessValidationTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();

        var user = new User("Qrwerwer", "SDFwerwe", "sdsdwQWEqe13","MyEmail@mail.ru")
        {
            Photo = "dasd",
            FavoriteBook = "sadasd",
            FavoriteMovie = "asdasd"
        };
        
        var expectedResult = new OperationResult<Unit, IReadOnlyCollection<string>>(Unit.Instance);

        var addResult = await userService.AddUser(user);
        addResult.Should().BeEquivalentTo(expectedResult);
    }
}