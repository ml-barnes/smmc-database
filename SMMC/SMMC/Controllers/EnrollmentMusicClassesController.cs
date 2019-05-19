using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMMC.Models;
using SMMC.ViewModels;

namespace SMMC.Controllers
{
    public class EnrollmentMusicClassesController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public EnrollmentMusicClassesController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: EnrollmentMusicClasses
        public async Task<IActionResult> Index()
        {
            var IN705_2018S2_db3shared01Context = _context.EnrollmentMusicClass.Include(e => e.Enrollment).Include(e => e.MusicClass);
            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: EnrollmentMusicClasses/Details/5
        public async Task<IActionResult> Details(int id1, int id2 )
        {
            var enrollmentMusicClass = await _context.EnrollmentMusicClass
                .Include(e => e.Enrollment)
                .Include(e => e.MusicClass)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id1 && m.MusicClassId == id2);
            if (enrollmentMusicClass == null)
            {
                return NotFound();
            }

            return View(enrollmentMusicClass);
        }

        // GET: EnrollmentMusicClasses/Create
        public IActionResult Create()
        {
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollment, "EnrollmentId", "EnrollmentId");
            ViewData["MusicClassId"] = new SelectList(_context.MusicClass, "MusicClassId", "MusicClassId");
            return View();
        }

        public IActionResult AddStudent(int id)
        {
            string proc = "GetEnrollment " + id.ToString(); 
            var enrollments = _context.Enrollment.FromSql(proc);
            IList<ViewModel> vm = new List<ViewModel>();

            foreach (var i in enrollments.ToList())
            {
                var en = _context.Enrollment.Include(e=>e.Student)
                    .ThenInclude(s=>s.Person)
                    .FirstOrDefault(a => a.StudentId == i.StudentId);
                vm.Add(new ViewModel { id = en.EnrollmentId, name = en.Student.Person.FirstName + " " + en.Student.Person.LastName });
            } 

            ViewData["Enrollment"] = new SelectList(vm, "id", "name");
            ViewData["ID"] = id;

            if (vm.Count == 0)
            {
                ViewData["Error"] = "No enrollments suitable for this class.";
            }           

            return View();
        }

        // POST: EnrollmentMusicClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(EnrollmentMusicClass enrollmentMusicClass)
        {
            enrollmentMusicClass.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(enrollmentMusicClass);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "MusicClasses", new { id = enrollmentMusicClass.MusicClassId } );
            }
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollment, "EnrollmentId", "EnrollmentId", enrollmentMusicClass.EnrollmentId);
            ViewData["MusicClassId"] = new SelectList(_context.MusicClass, "MusicClassId", "MusicClassId", enrollmentMusicClass.MusicClassId);
            return View(enrollmentMusicClass);
        }








        // POST: EnrollmentMusicClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentId,MusicClassId,Date")] EnrollmentMusicClass enrollmentMusicClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollmentMusicClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollment, "EnrollmentId", "EnrollmentId", enrollmentMusicClass.EnrollmentId);
            ViewData["MusicClassId"] = new SelectList(_context.MusicClass, "MusicClassId", "MusicClassId", enrollmentMusicClass.MusicClassId);
            return View(enrollmentMusicClass);
        }

        // GET: EnrollmentMusicClasses/Edit/5
        public async Task<IActionResult> Edit(int id1, int id2)
        {
            
            var enrollmentMusicClass = await _context.EnrollmentMusicClass.FindAsync(id1, id2);
            if (enrollmentMusicClass == null)
            {
                return NotFound();
            }
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollment, "EnrollmentId", "EnrollmentId", enrollmentMusicClass.EnrollmentId);
            ViewData["MusicClassId"] = new SelectList(_context.MusicClass, "MusicClassId", "MusicClassId", enrollmentMusicClass.MusicClassId);
            return View(enrollmentMusicClass);
        }

        // POST: EnrollmentMusicClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("EnrollmentId,MusicClassId,Date")] EnrollmentMusicClass enrollmentMusicClass)
        {

            System.Diagnostics.Debug.WriteLine(enrollmentMusicClass.EnrollmentId);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollmentMusicClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentMusicClassExists(enrollmentMusicClass.EnrollmentId))
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
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollment, "EnrollmentId", "EnrollmentId", enrollmentMusicClass.EnrollmentId);
            ViewData["MusicClassId"] = new SelectList(_context.MusicClass, "MusicClassId", "MusicClassId", enrollmentMusicClass.MusicClassId);
            return View(enrollmentMusicClass);
        }

        // GET: EnrollmentMusicClasses/Delete/5
        public async Task<IActionResult> Delete(int id1, int id2)
        { 
            var enrollmentMusicClass = await _context.EnrollmentMusicClass
                .Include(e => e.Enrollment)
                .Include(e => e.MusicClass)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id1 && m.MusicClassId == id2);
            if (enrollmentMusicClass == null)
            {
                return NotFound();
            }

            return View(enrollmentMusicClass);
        }

        // POST: EnrollmentMusicClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        { 
            int id1 = Convert.ToInt32(Request.Form["EnrollmentID"]);
            int id2 = Convert.ToInt32(Request.Form["MusicClassID"]);

            var enrollmentMusicClass = await _context.EnrollmentMusicClass
                .Include(e => e.Enrollment)
                .Include(e => e.MusicClass)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id1 && m.MusicClassId == id2);

            _context.EnrollmentMusicClass.Remove(enrollmentMusicClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentMusicClassExists(int id)
        {
            return _context.EnrollmentMusicClass.Any(e => e.EnrollmentId == id);
        }
    }
}
