using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabProject.Data;
using LabProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace LabProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        public IList<Class> ClassList { get; set; } = new List<Class>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // ✅ Login kontrolü
            if (HttpContext.Session.GetString("Username") == null)
            {
                TempData["ErrorMessage"] = "Please log in first.";
                return RedirectToPage("/Login");
            }

            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
                query = query.Where(c => c.Name.Contains(SearchTerm));

            int totalCount = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

            ClassList = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }

        public IActionResult OnPostExport(string? searchTerm)
        {
            var data = _context.Classes
                .Where(c => string.IsNullOrEmpty(searchTerm) || c.Name.Contains(searchTerm))
                .Select(c => new { c.Name, c.PersonCount, c.Description, c.IsActive })
                .ToList();

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            return File(Encoding.UTF8.GetBytes(json), "application/json", "classes.json");
        }
    }
}
