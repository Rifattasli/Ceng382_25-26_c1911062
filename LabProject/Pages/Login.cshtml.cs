using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabProject.Models;
using System.Text.Json;

namespace LabProject.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public bool LoginFailed { get; set; }

      public async Task<IActionResult> OnPostAsync()
{
    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/users.json");

    if (!System.IO.File.Exists(filePath))
    {
        LoginFailed = true;
        return Page();
    }

    string json = await System.IO.File.ReadAllTextAsync(filePath);
    var users = JsonSerializer.Deserialize<List<User>>(json);

    var user = users?.FirstOrDefault(u =>
        u.Username == Username &&
        u.Password == Password &&
        u.IsActive);

    if (user != null)
    {
        string token = Guid.NewGuid().ToString();

        HttpContext.Session.SetString("Username", user.Username);
        HttpContext.Session.SetString("Token", token);
        HttpContext.Session.SetString("SessionId", HttpContext.Session.Id);

        var cookieOptions = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddMinutes(30),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("Username", user.Username, cookieOptions);
        Response.Cookies.Append("Token", token, cookieOptions);
        Response.Cookies.Append("SessionId", HttpContext.Session.Id, cookieOptions);

        return RedirectToPage("/Index");
    }

    LoginFailed = true;
    return Page();
}
    }
}