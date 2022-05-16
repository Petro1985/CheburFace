using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SkillFactoryModule19.BLL;
using SkillFactoryModule19.BLL.Models;

namespace CheburFace.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserService _userService;

    public string Message { get; set; }

    public IndexModel(ILogger<IndexModel> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
        Message = "";
    }

    public IActionResult OnGet()
    {
        var userId = Request.Cookies.GetCurrentUserId();
        if (userId != 0)
        {
            return RedirectToPage("/UserServiceUI/MainPage");
        }
        return Page();
    }

    public async Task<IActionResult> OnPost(UserAuthenticationData userData)
    {
        var result = await _userService.AuthenticateUser(userData);

        if (result.IsSuccessful)
        {
            _logger.LogInformation("Login is successful");
            Response.Cookies.SetCurrentUserId(result.Result!.Id);
            return RedirectToPage("/UserServiceUI/MainPage");
        }
        else
        {
            _logger.LogInformation("Login failed");
            Message = result.Error + ". Try again.";
            return Page();
        }
        
    }
}

public static class IResponseCookiesExt
{
    private const string LoggedInUserId = "LoggedInUserId";

    public static void SetCurrentUserId(this IResponseCookies cookies, int? id)
    {
        if (id is null)
        {
            cookies.Delete(LoggedInUserId);
        }
        else
        {
            cookies.Append(LoggedInUserId, id.ToString());
        }
    }
    public static int GetCurrentUserId(this IRequestCookieCollection cookies)
    {
        int.TryParse(cookies[LoggedInUserId] ?? "0", out var id);
        return id;
    }}