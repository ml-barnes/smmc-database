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
    public class MusicClassesController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public MusicClassesController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: MusicClasses
        public async Task<IActionResult> Index()
        {
            var IN705_2018S2_db3shared01Context = _context.EnrollmentMusicClass
                .Include(m => m.MusicClass)
                    .ThenInclude(l => l.LessonTime)
                .Include(m => m.Enrollment)
                    .ThenInclude(e => e.Instrument)
                .Include(m => m.MusicClass)
                    .ThenInclude(t => t.Tutor.Staff.Person).GroupBy(e => e.MusicClassId).Select(g=>g.First());
            
            
            
            //.MusicClass
            //    .Include(m => m.LessonTime)
            //    .Include(m => m.Tutor.Staff.Person)
            //    .Include(m=>m.EnrollmentMusicClass)
            //    .Include(m=>m.Tutor.TutorType);

            


            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: MusicClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = _context.EnrollmentMusicClass
                .Where(emc => emc.MusicClassId == id)
                .Include(e => e.Enrollment.Instrument)
                .Include(e => e.Enrollment.Student.Person)
                .Include(e=>e.MusicClass.Tutor.Staff.Person)
                .ToList();
            

            ViewBag.Grade = students.First().Enrollment.Grade;
            ViewBag.Instrument = students.First().Enrollment.Instrument.Instrument1;
           

            var musicClass = await _context.MusicClass
                .Include(m => m.LessonTime)
                .Include(m => m.Tutor)                
                .FirstOrDefaultAsync(m => m.MusicClassId == id); 
                
            if (musicClass == null)
            {
                return NotFound();
            }

            //ViewData["test"] = new SelectList(s);
            return View(musicClass);
        }

        // GET: MusicClasses/Create
        public IActionResult Create()
        {
            ViewData["LessonTimeId"] = new SelectList(_context.LessonTime, "LessonTimeId", "LessonTimeId");
            ViewData["TutorId"] = new SelectList(_context.Tutor, "TutorId", "TutorId");
            return View();
        }

        // POST: MusicClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MusicClassId,TutorId,LessonTimeId,StartDate,EndDate")] MusicClass musicClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musicClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonTimeId"] = new SelectList(_context.LessonTime, "LessonTimeId", "LessonTimeId", musicClass.LessonTimeId);
            ViewData["TutorId"] = new SelectList(_context.Tutor, "TutorId", "TutorId", musicClass.TutorId);
            return View(musicClass);
        }

        // GET: MusicClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicClass = await _context.MusicClass.FindAsync(id);
            if (musicClass == null)
            {
                return NotFound();
            }

            var students = _context.EnrollmentMusicClass
                .Where(emc => emc.MusicClassId == id)
                .Include(e => e.Enrollment.Instrument)
                .Include(e => e.Enrollment.Student.Person)
                .Include(e => e.MusicClass.Tutor.Staff.Person)
                .ToList();


            List<Tutor> tutors = _context.Tutor.Include(a => a.Staff.Person).ToList();

            List<TutorViewModel> t = tutors.Select(s => new TutorViewModel
            {
                id = s.TutorId,
                name = s.Staff.Person.FirstName + " " + s.Staff.Person.LastName

            }).ToList();

            ViewData["LessonTimeId"] = new SelectList(_context.LessonTime, "LessonTimeId", "Time", musicClass.LessonTimeId);
            ViewData["TutorId"] = new SelectList(t, "id", "name", musicClass.TutorId);
            return View(musicClass);
        }

        // POST: MusicClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MusicClassId,TutorId,LessonTimeId,StartDate,EndDate")] MusicClass mc)
        {
            var musicClass = await _context.MusicClass
               .Include(m => m.LessonTime)
               .Include(m => m.Tutor)
               .FirstOrDefaultAsync(m => m.MusicClassId == id);

            musicClass.LessonTimeId = mc.LessonTimeId;
            musicClass.TutorId = mc.TutorId;


            if (id != musicClass.MusicClassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musicClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicClassExists(musicClass.MusicClassId))
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
            ViewData["LessonTimeId"] = new SelectList(_context.LessonTime, "LessonTimeId", "LessonTimeId", musicClass.LessonTimeId);
            ViewData["TutorId"] = new SelectList(_context.Tutor, "TutorId", "TutorId", musicClass.TutorId);
            return View(musicClass);
        }

        // GET: MusicClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicClass = await _context.MusicClass
                .Include(m => m.LessonTime)
                .Include(m => m.Tutor)
                .FirstOrDefaultAsync(m => m.MusicClassId == id);
            if (musicClass == null)
            {
                return NotFound();
            }

            return View(musicClass);
        }

        // POST: MusicClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musicClass = await _context.MusicClass.FindAsync(id);
            _context.MusicClass.Remove(musicClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteStudent(int id1, int id2)
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
        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudentConfirmed()
        {
            int id1 = Convert.ToInt32(Request.Form["EnrollmentID"]);
            int id2 = Convert.ToInt32(Request.Form["MusicClassID"]);

            var enrollmentMusicClass = await _context.EnrollmentMusicClass
                .Include(e => e.Enrollment)
                .Include(e => e.MusicClass)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id1 && m.MusicClassId == id2);

            _context.EnrollmentMusicClass.Remove(enrollmentMusicClass);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = enrollmentMusicClass.MusicClassId });
        }


        private bool MusicClassExists(int id)
        {
            return _context.MusicClass.Any(e => e.MusicClassId == id);
        }
    }
}
