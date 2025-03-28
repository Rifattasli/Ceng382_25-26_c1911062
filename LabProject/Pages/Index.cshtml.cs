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

        [BindProperty(SupportsGet = true)]
        public int? EditId { get; set; }

        public void OnGet()
        {
            if (EditId.HasValue)
            {
                var item = ClassList.FirstOrDefault(c => c.Id == EditId.Value);
                if (item != null)
                {
                    NewClass = new ClassInformationModel
                    {
                        Id = item.Id,
                        ClassName = item.ClassName,
                        StudentCount = item.StudentCount,
                        Description = item.Description
                    };
                }
            }
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
                return Page();

            // Elle ID ver
            NewClass.Id = ClassInformationModel.GenerateId();

            ClassList.Add(NewClass);
            return RedirectToPage();
        }

        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid)
                return Page();

            var existing = ClassList.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existing != null)
            {
                existing.ClassName = NewClass.ClassName;
                existing.StudentCount = NewClass.StudentCount;
                existing.Description = NewClass.Description;
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = ClassList.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                ClassList.Remove(item);
            }
            return RedirectToPage();
        }
    }
}