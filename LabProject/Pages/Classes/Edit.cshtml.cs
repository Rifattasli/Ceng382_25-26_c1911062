using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabProject.Data;
using LabProject.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LabProject.Pages.Classes
{
    public class EditModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public EditModel(SchoolDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Class Class { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Class = await _context.Classes.FindAsync(id);
            if (Class == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(Class).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}