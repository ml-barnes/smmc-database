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
    public class InstrumentsController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public InstrumentsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Instruments
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.Instrument.Include(i => i.HeadTutorNavigation).ThenInclude(h => h.Staff)
                .ThenInclude(s => s.Person);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Instruments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument
                .Include(i => i.HeadTutorNavigation)
                .FirstOrDefaultAsync(m => m.InstrumentId == id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
        }

        // GET: Instruments/Create
        public IActionResult Create()
        {
            List<Tutor> tutors = _context.Tutor.Include(g => g.Staff).ThenInclude(s => s.Person).ToList();
            List<NameViewModel> tutorNames = tutors.Select(tutor => new NameViewModel
            {
                id = tutor.TutorId,
                name = tutor.Staff.Person.FirstName + " " + tutor.Staff.Person.LastName

            }).ToList();
            ViewData["HeadTutor"] = new SelectList(tutorNames, "id", "name");
            return View();
        }

        // POST: Instruments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentId,Instrument1,StudentFee,OpenFee,HireFee,HeadTutor")] Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instrument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HeadTutor"] = new SelectList(_context.Tutor, "TutorId", "TutorId", instrument.HeadTutor);
            return View(instrument);
        }

        // GET: Instruments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument.FindAsync(id);
            if (instrument == null)
            {
                return NotFound();
            }
            

            List<Tutor> tutors =  _context.Tutor.Include(p => p.Staff.Person).ToList();

            List<ViewModel> tuts = tutors.Select(tutor => new ViewModel
            {
                id = tutor.TutorId,
                name = tutor.Staff.Person.FirstName + " " + tutor.Staff.Person.LastName

            }).ToList();




            ViewData["HeadTutor"] = new SelectList(tuts, "id", "name", instrument.HeadTutor);
            return View(instrument);
        }

        // POST: Instruments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentId,Instrument1,StudentFee,OpenFee,HireFee,HeadTutor")] Instrument instrument)
        {
            if (id != instrument.InstrumentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentExists(instrument.InstrumentId))
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
            ViewData["HeadTutor"] = new SelectList(_context.Tutor, "TutorId", "TutorId", instrument.HeadTutor);
            return View(instrument);
        }

        // GET: Instruments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrument = await _context.Instrument
                .Include(i => i.HeadTutorNavigation)
                .FirstOrDefaultAsync(m => m.InstrumentId == id);
            if (instrument == null)
            {
                return NotFound();
            }

            return View(instrument);
        }

        // POST: Instruments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrument = await _context.Instrument.FindAsync(id);
            _context.Instrument.Remove(instrument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstrumentExists(int id)
        {
            return _context.Instrument.Any(e => e.InstrumentId == id);
        }
    }
}
