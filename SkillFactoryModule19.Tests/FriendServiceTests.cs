using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;
using Xunit;

namespace SkillFactoryModule19.Tests;

public class FriendServiceTests
{
    [Fact]
    public async Task AddFriendTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();
        var friendService = provider.GetRequiredService<FriendService>();

        var user1 = new User("TestUserFN", "TestUserLN", "qwe123QWE!@#", "Petr@list.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };

        var user2 = new User("TestUserFN", "TestUserLN", "qwe123QWE!@#", "MyEmail@gmail.com")
        {
            Photo = "!The Photo!",
            FavoriteBook = "Haven't decided yet",
            FavoriteMovie = "Terminator 4",
        };

        await userService.AddUser(user1);
        await userService.AddUser(user2);

        var result = await friendService.AddFriend(new Friend(1, 2));

        result.IsSuccessful.Should().BeTrue();

    }
}