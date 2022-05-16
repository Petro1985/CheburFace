using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CheburFace.Pages;

public class LogOut : PageModel
{
    public IActionResult OnGet()
    {
        Response.Cookies.SetCurrentUserId(null);
        Response.Cookies.Delete("FriendFilter");
        Response.Cookies.Delete("RecipientEmail");
        return RedirectToPage("/Index");
    }
}