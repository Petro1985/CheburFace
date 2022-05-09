using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;
using Xunit;

namespace SkillFactoryModule19.Tests;

public class MessageServiceTests
{
    [Fact]
    public async Task SuccesfulCreateMessageTest()
    {
        var provider = new TestCheburfaceServiceProvider();
        var userService = provider.GetRequiredService<UserService>();
        var messageService = provider.GetRequiredService<MessageService>();

        var newUserFrom = new User("TestUserFN", "TestUserLN", "qwe123QWE!@#", "Petr@list.ru")
        {
            Photo = "Scenery",
            FavoriteBook = "Harry Potter",
            FavoriteMovie = "Terminator 2"
        };
        var newUserTo = new User("TestUserFN", "TestUserLN", "qwe123QWE!@#", "MyEmail@gmail.com")
        {
            Photo = "!The Photo!",
            FavoriteBook = "Haven't decided yet",
            FavoriteMovie = "Terminator 4",
        };

        await userService.AddUser(newUserFrom);
        await userService.AddUser(newUserTo);
        
        var userFrom = await userService.GetUser("Petr@list.ru");
        var userTo = await userService.GetUser("MyEmail@gmail.com");
        
        var message = new Message ("Test message from Petr@list.ru to MyEmail@gmail.com", userFrom!.Id,userTo!.Id);
        message.RecipientEMail = "MyEmail@gmail.com";
        
        var result = await messageService.Send(message);

        result.IsSuccessful.Should().BeTrue();

    }
}