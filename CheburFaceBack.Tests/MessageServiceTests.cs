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
    public async Task SuccessfulCreateMessageTest()
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
        
        var message = new Message("Test message from Petr@list.ru to MyEmail@gmail.com", 1, 4);

        var result = await messageService.Send(message);

        result.IsSuccessful.Should().BeTrue();

    }
    
    [Fact]
    public async Task EmptyContentMessageTest()
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
        
        var message = new Message("", 1, 2);

        var result = await messageService.Send(message);

        result.IsSuccessful.Should().BeFalse();
    }    
    
    [Fact]
    public async Task GetMessagesTest()
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
        
        await messageService.Send(new Message("23", 1, 2));
        await messageService.Send(new Message("123", 1, 2));
        await messageService.Send(new Message("123", 1, 2));
        await messageService.Send(new Message("sad", 1, 2));
        await messageService.Send(new Message("asd", 1, 2));
        await messageService.Send(new Message("asd", 1, 2));

        var result = await messageService.GetMessagesByRecipient(2);

        result.Count.Should().Be(6);
    }    
        
    [Fact]
    public async Task GetMessagesFromSenderTest()
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
        
        var newUserFrom2 = new User("TestUserFN", "TestUserLN", "qwe123QWE!@#", "Nastiia@list.ru")
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
        await userService.AddUser(newUserFrom2);
        await userService.AddUser(newUserTo);

        await messageService.Send(new Message("23", 1, 3));
        await messageService.Send(new Message("123", 1, 3));
        await messageService.Send(new Message("123", 1, 3));
        await messageService.Send(new Message("sad", 1, 3));
        await messageService.Send(new Message("asd", 2, 3));
        await messageService.Send(new Message("asd", 2, 3));
        await messageService.Send(new Message("asd", 1, 2));
        await messageService.Send(new Message("asd", 1, 2));

        var result = await messageService.GetMessages(1);

        result.Count.Should().Be(6);
    }    
}