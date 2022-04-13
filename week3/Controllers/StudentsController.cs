#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using week3.Models;

namespace week3.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;
        private readonly int _recordsPerPage = 5;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int id, string searchString="")
        {
            var students = _context.Students
                .Where(s => s.LastName.Contains(searchString) || s.FirstMidName.Contains(searchString));
            int numOfRecord = students.Count();
            ViewBag.numberOfPages = (int)Math.Ceiling((double)numOfRecord / _recordsPerPage);
            ViewBag.currentPage = id;
            List<Student> studentsList = await students.Skip(id*_recordsPerPage)
                .Take(numOfRecord).ToListAsync();
            return View(studentsList);
        }
        //public async Task<IActionResult> Index(int id = 0)
        //{
        //    int numberOfRecords = await _context.Students.CountAsync();     //Count SQL
        //    int numberOfPages = (int)Math.Ceiling((double)numberOfRecords / _recordsPerPage);
        //    ViewBag.numberOfPages = numberOfPages;
        //    ViewBag.currentPage = id;
        //    //List<Student> students = await _context.Students.Where(s => s.LastName == "Alexander").ToListAsync(); // where LastName ==
        //    //List<Student> students = await _context.Students.Where(s => s.LastName.StartsWith("a")).ToListAsync(); // where LastName Like
        //    //List<Student> students = await _context.Students.Where(s => EF.Functions.Like(s.LastName, "a%")).ToListAsync(); // similarly

        //    //List<Course> courses = await _context.Courses
        //    //                .Where(c => c.Credits > 2 && c.Title.EndsWith("economics")) // more than 1 condition
        //    //                .ToListAsync();

        //    List<Student> students = await _context.Students
        //        .Skip(id * _recordsPerPage)  //Offset SQL
        //        .Take(_recordsPerPage)       //Top SQL
        //        .ToListAsync();
        //    return View(students);
        //}
        //public async Task<IActionResult> Index(string sortOrder)
        //{
        //    ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
        //    var students = from s in _context.Students
        //                   select s;
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }
        //    return View(await students.AsNoTracking().ToListAsync());
        //}
        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(student); //Add one or more right here.
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Error is: " + ex.Message);
            }
            return View(student);

        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
                if (studentToUpdate == null)
                {
                    return NotFound();
                }
                studentToUpdate.FirstMidName = student.FirstMidName;
                studentToUpdate.LastName = student.LastName;
                studentToUpdate.EnrollmentDate = student.EnrollmentDate;
                try
                {
                    _context.Update(studentToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError("", "Unable to update the change. Error is: " + ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }


        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to delete student " + id + ". Error is: " + ex.Message);
                return NotFound();
            }
        }

    }
}
