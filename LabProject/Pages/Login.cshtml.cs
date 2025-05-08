using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using LabProject.Data;
using LabProject.Models;
using System.Linq;

namespace LabProject.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public LoginModel(SchoolDbContext context)
        {
            _context = context;
        }

        // ✅ FORM verisini bağlamak için
        [BindProperty]
        public InputModel Input { get; set; }

        // ✅ Hatalı giriş kontrolü için
        public bool ShowLoginError { get; set; }

        // ✅ İç sınıf: formda username ve password tutulur
        public class InputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public IActionResult OnPost()
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == Input.Username &&
                u.Password == Input.Password &&
                u.IsActive);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("SessionId", HttpContext.Session.Id);

                Response.Cookies.Append("Username", user.Username);
                Response.Cookies.Append("Token", "token-value");
                Response.Cookies.Append("SessionId", HttpContext.Session.Id);

                return RedirectToPage("/Index");
            }

            ShowLoginError = true;
            return Page();
        }
    }
}