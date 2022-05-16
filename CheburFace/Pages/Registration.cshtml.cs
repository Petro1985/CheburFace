using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;

namespace CheburFace.Pages;

public class RegistrationModel : PageModel
{
    private readonly ILogger<RegistrationModel> _logger;
    private readonly UserService _userService;

    [BindProperty] public User UserData { get; set; } = new ("", "", "", "");

    [BindProperty] public string PasswordRepetition { get; set; } = "";

    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

    public RegistrationModel(ILogger<RegistrationModel> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public void OnGet()
    {
        
    }

    public async Task<IActionResult> OnPost()
    {
        if (UserData.Password != PasswordRepetition)
        {
            Errors = new List<string>() {"Password confirmation failed. Try again."};
            return Page();
        }
        
        var result = await _userService.AddUser(UserData);

        if (!result.IsSuccessful)
        {
            Errors = result.Error ?? Enumerable.Empty<string>();
            _logger.LogInformation("Registration failed Email: {User} with errors: {@Errors}", UserData.EMail, Errors);
        }
        else
        {
            _logger.LogInformation("Registered new User {EMail}", UserData.EMail);
        }

        var newUser = await _userService.GetUser(UserData.EMail);
        Response.Cookies.SetCurrentUserId(newUser.Id);
        return RedirectToPage("/UserServiceUI/MainPage");
    }

    public void OnSingUp()
    {
        
    }
}