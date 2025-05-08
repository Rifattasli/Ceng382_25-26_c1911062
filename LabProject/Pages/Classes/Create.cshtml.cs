using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabProject.Data;
using LabProject.Models;

namespace LabProject.Pages.Classes
{
    public class CreateModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public CreateModel(SchoolDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Class Class { get; set; } = new();

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Classes.Add(Class);
            _context.SaveChanges();

            return RedirectToPage("/Index");
        }
    }
}