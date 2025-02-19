using Microsoft.EntityFrameworkCore;
using DatePickerHint.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DatePickerHint.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        //adding a program model
        public DbSet<StudyProgram> Programs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            // Seed StudyPrograms
            modelBuilder.Entity<StudyProgram>().HasData(
                new StudyProgram { ProgramCode = "CP", Name = "Computer Programming" },
            /*    new StudyProgram { ProgramCode = "CPA", Name = "Computer Programmer Analyst" },*/
                new StudyProgram { ProgramCode = "BACS", Name = "Bachelor of Computer Science" }
            );
            // Define relationship between Student and StudyProgram
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Program)
                .WithMany(p => p.Students)
                .HasForeignKey(s => s.ProgramCode)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);


            // Seed Students (ensure ProgramCode is valid according to the seeded data)
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentId = 1,
                    FirstName = "Bart",
                    LastName = "Simpson",
                    DOB = new DateTime(1971, 5, 31),
                    GPA = 2.7,
                    ProgramCode = "CP"
                },
                new Student
                {
                    StudentId = 2,
                    FirstName = "Lisa",
                    LastName = "Simpson",
                    DOB = new DateTime(1973, 8, 5),
                    GPA = 4.0,
                    ProgramCode = "BACS"
                }
                    );
        }
    }
}