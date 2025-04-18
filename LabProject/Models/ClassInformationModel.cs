using System.ComponentModel.DataAnnotations;

namespace LabProject.Models
{
    public class ClassInformationModel
    {
        private static int _nextId = 1;

        public int Id { get; set; }

        [Required(ErrorMessage = "Class name is required")]
        public string ClassName { get; set; } =string.Empty;

        [Range(1, 100, ErrorMessage = "Student count must be between 1 and 100")]
        public int StudentCount { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }=string.Empty;

        public static int GenerateId()
        {
            return _nextId++;
        }
    }
}