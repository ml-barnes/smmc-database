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
    public class SheetMusicTutorsController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public SheetMusicTutorsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: SheetMusicTutors
        public async Task<IActionResult> Index()
        {
            var IN705_2018S2_db3shared01Context = _context.SheetMusicTutor.Include(s => s.SheetMusic).Include(s => s.Tutor.Staff.Person);
            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: SheetMusicTutors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheetMusicTutor = await _context.SheetMusicTutor
                .Include(s => s.SheetMusic)
                .Include(s => s.Tutor)
                .FirstOrDefaultAsync(m => m.SheetMusicTutorId == id);
            if (sheetMusicTutor == null)
            {
                return NotFound();
            }

            return View(sheetMusicTutor);
        }

        // GET: SheetMusicTutors/Create
        public IActionResult Create()
        {
            List<SheetMusic> sheetMusic = _context.SheetMusic.ToList();

            List<SheetMusicViewModel> sms = sheetMusic.Select(s => new SheetMusicViewModel
            {
                id = s.SheetMusicId,
                name = s.Title + " (" + s.Composer + ") - Quantity: " + s.Amount

            }).ToList();

            ViewData["SheetMusic"] = new SelectList(sms, "id", "name");

            List<Tutor> tutors = _context.Tutor.Include(a=>a.Staff.Person).ToList();

            List<TutorViewModel> t = tutors.Select(s => new TutorViewModel
            {
                id = s.TutorId,
                name = s.Staff.Person.FirstName + " " + s.Staff.Person.LastName

            }).ToList();

            ViewData["Tutor"] = new SelectList(t, "id", "name");

            //ViewData["SheetMusicId"] = new SelectList(_context.SheetMusic, "SheetMusicId", "Title");
            //ViewData["TutorId"] = new SelectList(_context.Tutor, "TutorId", "TutorId");
            return View();
        }

        // POST: SheetMusicTutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SheetMusicTutorId,SheetMusicId,TutorId,AmountLoaned")] SheetMusicTutor sheetMusicTutor)
        {
            sheetMusicTutor.DateLoaned = DateTime.Now;
            SheetMusic sm = await _context.SheetMusic.FirstOrDefaultAsync(s => s.SheetMusicId == sheetMusicTutor.SheetMusicId);
            sm.Amount -= sheetMusicTutor.AmountLoaned;

            if (sm.Amount >= 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(sm);
                    _context.Add(sheetMusicTutor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ViewData["Error"] = "Not enough copies to loan. Can only loan out copies in stock.";
            }

            List<SheetMusic> sheetMusic = _context.SheetMusic.ToList();

            List<SheetMusicViewModel> sms = sheetMusic.Select(s => new SheetMusicViewModel
            {
                id = s.SheetMusicId,
                name = s.Title + " (" + s.Composer + ") - Quantity: " + (sm.Amount += sheetMusicTutor.AmountLoaned)

            }).ToList();

            ViewData["SheetMusic"] = new SelectList(sms, "id", "name");

            List<Tutor> tutors = _context.Tutor.Include(a => a.Staff.Person).ToList();

            List<TutorViewModel> t = tutors.Select(s => new TutorViewModel
            {
                id = s.TutorId,
                name = s.Staff.Person.FirstName + " " + s.Staff.Person.LastName

            }).ToList();

            ViewData["Tutor"] = new SelectList(t, "id", "name");

            return View(sheetMusicTutor);
        }

        // GET: SheetMusicTutors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheetMusicTutor = await _context.SheetMusicTutor.FindAsync(id);
            
            if (sheetMusicTutor == null)
            {
                return NotFound();
            }

            SheetMusic sm = await _context.SheetMusic.FirstOrDefaultAsync(s => s.SheetMusicId == sheetMusicTutor.SheetMusicId);
            var title = sm.Title;

            Tutor tutor = await _context.Tutor.Include(t => t.Staff.Person).FirstOrDefaultAsync(a => a.TutorId == sheetMusicTutor.TutorId);

            ViewData["SheetMusic"] = title;
            ViewData["name"] = tutor.Staff.Person.FirstName + " " + tutor.Staff.Person.LastName;
            return View(sheetMusicTutor);
        }

        // POST: SheetMusicTutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SheetMusicTutorId,SheetMusicId,TutorId,AmountLoaned")] SheetMusicTutor smt)
        {
            SheetMusicTutor sheetMusicTutor = await _context.SheetMusicTutor.FirstOrDefaultAsync(s => s.SheetMusicTutorId == id);
            sheetMusicTutor.DateReturned = DateTime.Now;

            SheetMusic sm = await _context.SheetMusic.FirstOrDefaultAsync(s => s.SheetMusicId == sheetMusicTutor.SheetMusicId);
            sm.Amount += sheetMusicTutor.AmountLoaned;


            if (id != sheetMusicTutor.SheetMusicTutorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sheetMusicTutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SheetMusicTutorExists(sheetMusicTutor.SheetMusicTutorId))
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
            ViewData["SheetMusicId"] = new SelectList(_context.SheetMusic, "SheetMusicId", "SheetMusicId", sheetMusicTutor.SheetMusicId);
            ViewData["TutorId"] = new SelectList(_context.Tutor, "TutorId", "TutorId", sheetMusicTutor.TutorId);
            return View(sheetMusicTutor);
        }

        // GET: SheetMusicTutors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {




            if (id == null)
            {
                return NotFound();
            }

            var sheetMusicTutor = await _context.SheetMusicTutor
                .Include(s => s.SheetMusic)
                .Include(s => s.Tutor)
                .FirstOrDefaultAsync(m => m.SheetMusicTutorId == id);
            if (sheetMusicTutor == null)
            {
                return NotFound();
            }

            return View(sheetMusicTutor);
        }

        // POST: SheetMusicTutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sheetMusicTutor = await _context.SheetMusicTutor.FindAsync(id);
            SheetMusic sm = await _context.SheetMusic.FirstOrDefaultAsync(s => s.SheetMusicId == sheetMusicTutor.SheetMusicId);

            sm.Amount += sheetMusicTutor.AmountLoaned;

            _context.SheetMusicTutor.Remove(sheetMusicTutor);
            await _context.SaveChangesAsync();

            try
            {
                _context.Update(sm);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SheetMusicTutorExists(sheetMusicTutor.SheetMusicTutorId))
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

        private bool SheetMusicTutorExists(int id)
        {
            return _context.SheetMusicTutor.Any(e => e.SheetMusicTutorId == id);
        }
    }
}
