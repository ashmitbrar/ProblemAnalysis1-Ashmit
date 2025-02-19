using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatePickerHint.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required]
        [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.0 and 4.0")]
        public double GPA { get; set; }

        // Foreign Key for StudyProgram
        [Required]
        [StringLength(10)]
        public string ProgramCode { get; set; }

        [ForeignKey("ProgramCode")]
        public StudyProgram Program { get; set; }

        //To get age we will compute it Using DOB as instructed. 

        [NotMapped]
        public int Age
        {
            get
            {
                return DateTime.Today.Year - DOB.Year -
                       (DateTime.Today.DayOfYear < DOB.DayOfYear ? 1 : 0);
            }
        }

        //For GPA using University of toronto GPA scale
        [NotMapped]
        public string GPAScale
        {
            get
            {
                if (GPA >= 3.7) return "Excellent";
                if (GPA >= 2.7) return "Very Good";
                if (GPA >= 1.7) return "Good";
                if (GPA >= 0.7) return "Satisfactory";
                return "Unsatisfactory";
            }
        }

    }
}
