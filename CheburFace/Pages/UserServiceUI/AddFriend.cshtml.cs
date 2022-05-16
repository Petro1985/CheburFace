using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;

namespace CheburFace.Pages.UserServiceUI;

public class AddFriend : PageModel
{
    private FriendService _friendService;
    private UserService _userService;
    private User? _currentUser;

    [BindProperty]
    public string Message { get; set; }

    public AddFriend(FriendService friendService, UserService userService)
    {
        _friendService = friendService;
        _userService = userService;
    }

    [BindProperty] public string FriendEmail { get; set; } = "";
    
    public async Task<IActionResult> OnGet()
    {
        var currentUserEmail = Request.Cookies.GetCurrentUserId();
        
        _currentUser = await _userService.GetUser(currentUserEmail);
        if (_currentUser is null)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }

    public async Task OnPost()
    {
        var friendUser = await _userService.GetUser(FriendEmail);
        if (friendUser is null)
        {
            Message = "Все пропало! Друга с такой почтой нет :-(";
            return;
        }

        var currentUserId = Request.Cookies.GetCurrentUserId();
        var result = await _friendService.AddFriend(new Friend(currentUserId, friendUser.Id));

        Message = !result.IsSuccessful ? result.Error! : "Друг добавлен!";
        
    }
}