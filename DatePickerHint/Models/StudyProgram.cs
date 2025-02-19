using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace DatePickerHint.Models
{
    public class StudyProgram
    {
        [Key]
        [Required]
        [StringLength(10)]
        public string ProgramCode { get; set; } // Primary Key (e.g., "CPA")

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // Program Name (e.g., "Computer Programmer Analyst")

        // Navigation property for students
        public ICollection<Student>? Students { get; set; }
    }
}
