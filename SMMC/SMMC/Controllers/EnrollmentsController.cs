using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMMC.Models;
using SMMC.ViewModels;
using System.Data.SqlClient;

namespace SMMC.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public EnrollmentsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var IN705_2018S2_db3shared01Context = _context.Enrollment.Include(e => e.Instrument).Include(e => e.Student).Include(e => e.Student.Person);
            
            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        public async Task<IActionResult> LoanError(String errorMessage, String color = "red")
        {            
            ViewData["LoanError"] = errorMessage;
            ViewData["Color"] = color;
            var IN705_2018S2_db3shared01Context = _context.Enrollment.Include(e => e.Instrument).Include(e => e.Student).Include(e => e.Student.Person);

            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Instrument)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            
            List<Student> students = _context.Student.Include(e => e.Person).ToList();

            List<StudentNameViewModel> studentName = students.Select(student => new StudentNameViewModel
            {
                id = student.StudentId,
                name = student.Person.FirstName + " " + student.Person.LastName

            }).ToList();

            List<int> grades = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };            

            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1");
            ViewData["Student"] = new SelectList(studentName, "id", "name");
            ViewData["Grade"] = new SelectList(grades);
            
            return View();
        }

        public IActionResult GenerateEnsembles()
        {
            string proc = "[AssignEnsembles]";
            var loanResult = _context.Database.ExecuteSqlCommand(proc);

            if(loanResult == 0)
            {
                return RedirectToAction("LoanError", new { errorMessage = "Generating ensembles failed" });
            }

            return RedirectToAction("LoanError", new { errorMessage = "Success!", color = "green"});
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {

            enrollment.Paid = false;
            if (Request.Form["paid"] == "1")
            {
                enrollment.Paid = true;
            }

            enrollment.Date = DateTime.Now;
            ViewData["LoanError"] = null;
            var test = 1;

            if (Request.Form["loan"] == "1")
            {
                //Insert into LoanStudent here
                string proc = "[Loan Instrument] " + enrollment.EnrollmentId.ToString();
                test = _context.Database.ExecuteSqlCommand(proc);                
            }

            
            try
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (e is SqlException | e is DbUpdateException)
                {
                    return RedirectToAction("LoanError", new { errorMessage = "Enrollment already exists." });
                }                    
            }                

            if (test < 1)
            {
                return RedirectToAction("LoanError", new { errorMessage = "No available instruments to loan." });
            }

            return RedirectToAction(nameof(Index));
            
            //ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1", enrollment.InstrumentId);
            //ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "StudentId", enrollment.StudentId);
            //System.Diagnostics.Debug.WriteLine("-----------------------------------------------------------     " + ViewData["LoanError"]);

            //return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Student> students = _context.Student.Include(e => e.Person).ToList();

            List<StudentNameViewModel> studentName = students.Select(student => new StudentNameViewModel
            {
                id = student.StudentId,
                name = student.Person.FirstName + " " + student.Person.LastName

            }).ToList();



            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1", enrollment.InstrumentId);
            ViewData["StudentId"] = new SelectList(studentName, "id", "name", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentId,StudentId,InstrumentId,Grade,Date,Paid")] Enrollment enrollment)
        {
            if (id != enrollment.EnrollmentId)
            {
                return NotFound();
            }       




            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    if (e is SqlException | e is DbUpdateException)
                    {
                        return RedirectToAction("LoanError", new { errorMessage = "Can't update Enrollment, enrollment already exists." });
                    }
                    if (!EnrollmentExists(enrollment.EnrollmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1", enrollment.InstrumentId);
            ViewData["StudentId"] = new SelectList(_context.Student, "StudentId", "StudentId", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Instrument)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.EnrollmentId == id);
        }

       
    }
}
