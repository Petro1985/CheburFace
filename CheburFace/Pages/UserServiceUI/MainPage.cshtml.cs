using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;

namespace CheburFace.Pages.UserServiceUI;

public class MainPage : PageModel
{
    private UserService _userService;
    private MessageService _messageService;
    private FriendService _friendService;
    
    public User? CurrentUser { get; set; }

    public List<PageMessages> Messages { get; set; }
    public List<User> Friends { get; set; } = new ();
    
    [BindProperty(SupportsGet = true)]
    public string Message { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string RecipientEmail { get; set; }

    [BindProperty(SupportsGet = true)]
    public IEnumerable<string>? ErrorMessage { get; set; } = ArraySegment<string>.Empty;
    
    [BindProperty(SupportsGet = true)]
    public int FriendFilter { get; set; }

    [BindProperty]
    public IFormFile Upload { get; set; }
    public MainPage(UserService userService, MessageService messageService, FriendService friendService)
    {
        _userService = userService;
        _messageService = messageService;
        _friendService = friendService;
    }

    public async Task<IActionResult> OnGet()
    {

        if (RecipientEmail is null)
        {
            RecipientEmail = Request.Cookies["RecipientEmail"];
        }
        else
        {
            Response.Cookies.Append("RecipientEmail", RecipientEmail);
        }

        if (FriendFilter == 0)
        {
             int.TryParse(Request.Cookies["FriendFilter"], out var parseResult);
             FriendFilter = parseResult;
        }
        else
        {
            Response.Cookies.Append("FriendFilter", FriendFilter.ToString());
        }

        
        var currentUserId = Request.Cookies.GetCurrentUserId();
        CurrentUser = await _userService.GetUser(currentUserId);
        if (CurrentUser is null)
        {
            return RedirectToPage("/Index");
        }

        Friends = await GetFriends();
        
        var taskMessages = (await GetMessages())
            .Where(i => FriendFilter == 0 
                        || i.SenderId == FriendFilter 
                        || (i.RecipientId == FriendFilter && i.SenderId == currentUserId) 
                        || (i.RecipientId == currentUserId && i.SenderId == FriendFilter))
            .Select(async message =>
        {
            var user = (await _userService.GetUser(message.SenderId));
            var recipient = (await _userService.GetUser(message.RecipientId));
            
            return new PageMessages()
            {
                Content = message.Content,
                Sender = user!.FirstName + " " + user.LastName,
                SenderEmail = user.EMail,
                IsFromMe = user.Id==CurrentUser.Id,
                Recipient = recipient!.FirstName + " " + recipient.LastName,
                Id = message.Id,
            };
        }).ToList();
        
        await Task.WhenAll(taskMessages);
        
        Messages = taskMessages.Select(item => item.Result).ToList();

        return Page();
    }

    private async Task<ICollection<Message>> GetMessages()
    {
        return await _messageService.GetMessages(CurrentUser!.Id);
    }

    public async Task<IActionResult> OnPostDeleteMessageAsync(int id)
    {
        await _messageService.ObliterateMessage(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendMessageAsync()
    {
        var recipient = await _userService.GetUser(RecipientEmail);
        if (recipient is null)
        {
            ErrorMessage = new List<string> {"Пользователя с такой почтой в Чебурнете нет :-("};
            return RedirectToPage(new {ErrorMessage = ErrorMessage});
        }

        if (String.IsNullOrWhiteSpace(Message))
        {
            ErrorMessage = new List<string> {"Сообщение не должно быть пустым."};
            return RedirectToPage(new {ErrorMessage = ErrorMessage});
        }

        var currentUserId = Request.Cookies.GetCurrentUserId();
        var result = await _messageService.Send(new Message(Message, currentUserId, recipient.Id));

        if (result.IsSuccessful)
        {
            ErrorMessage = new List<string> {"Сообщение отправлено!"};
        }
        else
        {
            ErrorMessage = result.Error;
        }

        Response.Cookies.Append("RecipientEmail",RecipientEmail);
        
        return RedirectToPage(
            new
            {
                ErrorMessage = new List<string>{"Сообщение отправлено"},
            });
    }

    public class PageMessages
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string SenderEmail { get; set; }
        public bool IsFromMe { get; set; }
    }

    private async Task<List<User>> GetFriends()
    {
        var tasksFriends = (await _friendService.GetFriends(CurrentUser.Id))
            .Select(async friend => (await _userService.GetUser(friend.FriendId))!).ToList();

        await Task.WhenAll(tasksFriends);
        return tasksFriends.Select(f => f.Result).ToList();
    }
    
    public async Task<IActionResult> OnPostLoadPhotoAsync()
    {
        var currentUserId = Request.Cookies.GetCurrentUserId();
        CurrentUser = await _userService.GetUser(currentUserId);

        if (CurrentUser is null)
        {
            return RedirectToPage("/Registration");
        }
        
        var file = Path.Combine("Photos", CurrentUser.Id.ToString());
        var stream = Upload.OpenReadStream();
        await _userService.SetPhoto(CurrentUser, stream);

        return RedirectToPage();
    }
}