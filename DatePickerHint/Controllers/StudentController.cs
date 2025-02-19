using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DatePickerHint.Data;
using DatePickerHint.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DatePickerHint.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }
        //list of all student
        public IActionResult Index()
        {
            // Check if Programs table has data
            var programs = _context.Programs.ToList();
            foreach (var p in programs)
            {
                Console.WriteLine($"ProgramCode: {p.ProgramCode}, Name: {p.Name}");
            }
            //getting all students with program info
            var students = _context.Students.Include(s => s.Program).ToList();
            return View(students);
        }


        //creating new student
        public IActionResult Create()
        {//dropdown for available programs
            ViewBag.Programs = new SelectList(_context.Programs, "ProgramCode", "Name");
            return View("AddStudent");
        }
        //creating new student-post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            { //adding student to database
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            // Reload programs in case of validation failure
            ViewBag.Programs = new SelectList(_context.Programs, "ProgramCode", "Name", student.ProgramCode);
            return View("AddStudent", student);
        }
        //editing 
   
        public IActionResult Edit(int id)
        {// getting student record
            var student = _context.Students
                                   .Include(s => s.Program) // Ensure Program data is loaded
                                   .FirstOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }
            // Load programs for the dropdown
            ViewBag.Programs = new SelectList(_context.Programs, "ProgramCode", "Name", student.ProgramCode);
            return View("EditStudent", student); // Directly passing the student model to the view
        }

        // POST: Edit Student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Make sure the existing Program is not being overwritten incorrectly
                    var existingStudent = await _context.Students.FindAsync(id);
                    if (existingStudent == null)
                    {
                        return NotFound();
                    }

                    // Update fields but preserve the Program relationship
                    existingStudent.FirstName = student.FirstName;
                    existingStudent.LastName = student.LastName;
                    existingStudent.DOB = student.DOB;
                    existingStudent.GPA = student.GPA;
                    existingStudent.ProgramCode = student.ProgramCode; // Ensure FK is updated

                    await _context.SaveChangesAsync(); // Asynchronous save
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");  // Redirect to Index after saving
            }
            // Reload programs if validation fails
            ViewBag.Programs = new SelectList(_context.Programs, "ProgramCode", "Name", student.ProgramCode);
            return View("EditStudent", student);  // Return to Edit view with validation errors
        }

        // StudentExists method to check if the student exists in the database
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }


        //delete student
        public IActionResult Delete(int id)
        {
            var student = _context.Students.Include(s => s.Program).FirstOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }
            return View("DeleteStudent", student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
