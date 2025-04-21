using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;

namespace LabProject.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> ClassList { get; set; } = new();
        public List<ClassInformationTable> FilteredList { get; set; } = new();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? FilterName { get; set; }

        [BindProperty(SupportsGet = true)]

        
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public List<string> SelectedColumns { get; set; } = new();

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        private static bool TestDataLoaded = false;

        public void OnGet()
        {
            string? cookieUsername = Request.Cookies["Username"];
            string? cookieToken = Request.Cookies["Token"];
            string? cookieSessionId = Request.Cookies["SessionId"];

            string? sessionUsername = HttpContext.Session.GetString("Username");
            string? sessionToken = HttpContext.Session.GetString("Token");
            string? sessionId = HttpContext.Session.GetString("SessionId");
            

            ViewData["SessionToken"] = sessionToken;
            ViewData["CookieToken"] = cookieToken;

            if (cookieUsername != sessionUsername ||
                cookieToken != sessionToken ||
                cookieSessionId != sessionId)
            {
                TempData["ErrorMessage"] = "Login required or session expired.";
                Response.Redirect("/Login");
                return;
            }

            if (!TestDataLoaded)
            {
                for (int i = 1; i <= 100; i++)
                {
                    ClassList.Add(new ClassInformationModel
                    {
                        Id = ClassInformationModel.GenerateId(),
                        ClassName = $"Class {i}",
                        StudentCount = 20 + (i % 10),
                        Description = $"Sample description {i}"
                    });
                }
                TestDataLoaded = true;
            }

            var query = ClassList.AsQueryable();

            if (!string.IsNullOrWhiteSpace(FilterName))
            {
                query = query.Where(c => c.ClassName.Contains(FilterName));
            }

            TotalPages = (int)System.Math.Ceiling(query.Count() / (double)PageSize);

            FilteredList = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(c => new ClassInformationTable
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                })
                .ToList();

            if (SelectedColumns.Count == 0)
            {
                SelectedColumns = new List<string> { "ClassName", "StudentCount", "Description" };
            }
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            NewClass.Id = ClassInformationModel.GenerateId();
            ClassList.Add(NewClass);

            return RedirectToPage(new
            {
                FilterName,
                PageNumber,
                SelectedColumns = string.Join(",", SelectedColumns)
            });
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
                ClassList.Remove(item);

            return RedirectToPage(new
            {
                FilterName,
                PageNumber,
                SelectedColumns = string.Join(",", SelectedColumns)
            });
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Token");
            Response.Cookies.Delete("SessionId");

            return RedirectToPage("/Login");
        }

        public IActionResult OnPostExport(string SelectedColumns, bool isFiltered, string? filterName, int PageNumber)
{
    var columns = SelectedColumns.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

    var query = isFiltered
        ? ClassList
            .Where(c => string.IsNullOrWhiteSpace(filterName) || c.ClassName.Contains(filterName))
        : ClassList.AsEnumerable();

    const int PageSize = 10;
    var pagedData = query
        .Skip((PageNumber - 1) * PageSize)
        .Take(PageSize)
        .Select(c => new ClassInformationTable
        {
            Id = c.Id,
            ClassName = c.ClassName,
            StudentCount = c.StudentCount,
            Description = c.Description
        })
        .ToList();

    var exportData = pagedData.Select(item =>
    {
        var obj = new Dictionary<string, object>();
        if (columns.Contains("ClassName")) obj["ClassName"] = item.ClassName;
        if (columns.Contains("StudentCount")) obj["StudentCount"] = item.StudentCount;
        if (columns.Contains("Description")) obj["Description"] = item.Description;
        return obj;
    });

    var json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
    var bytes = Encoding.UTF8.GetBytes(json);
    return File(bytes, "application/json", $"page{PageNumber}_export.json");
}
    }
}