using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace LabProject.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> ClassList = new();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        public List<ClassInformationTable> FilteredList { get; set; } = new();

        
        [BindProperty(SupportsGet = true)]
        public string? FilterName { get; set; }

        
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        private static bool TestDataLoaded = false;
        public void OnGet()
        {
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

            FilteredList = pagedData;
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
        PageNumber
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
        PageNumber
    });
}
    }
}