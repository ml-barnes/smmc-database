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
    public class TutorTypesController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public TutorTypesController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: TutorTypes
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.TutorType.Include(tt => tt.Instrument).Include(tt => tt.Tutor)
                .ThenInclude(t => t.Staff).ThenInclude(s => s.Person);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: TutorTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorType = await _context.TutorType
                .Include(tt => tt.Instrument)
                .Include(tt => tt.Tutor)
                .ThenInclude(t => t.Staff)
                .ThenInclude(s => s.Person)
                .FirstOrDefaultAsync(m => m.TutorTypeId == id);
            if (tutorType == null)
            {
                return NotFound();
            }

            return View(tutorType);
        }

        // GET: TutorTypes/Create
        public IActionResult Create()
        {
            List<Tutor> tutors = _context.Tutor.Include(t => t.Staff).ThenInclude(s => s.Person).ToList();
            List<NameViewModel> tutorNames = tutors.Select(tutor => new NameViewModel
            {
                id = tutor.TutorId,
                name = tutor.Staff.Person.FirstName + " " + tutor.Staff.Person.LastName

            }).ToList();
            ViewData["Tutor"] = new SelectList(tutorNames, "id", "name");

            List<Instrument> instruments = _context.Instrument.ToList();
            List<NameViewModel> instrumentNames = instruments.Select(instrument => new NameViewModel
            {
                id = instrument.InstrumentId,
                name = instrument.Instrument1

            }).ToList();
            ViewData["Instrument"] = new SelectList(instrumentNames, "id", "name");
            List<int> grades = new List<int> { 6, 8 };
            ViewData["Grade"] = new SelectList(grades);
            return View();
        }

        // POST: TutorTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TutorId,InstrumentId,MaxGrade")] TutorType tutorType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tutorType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1", tutorType.InstrumentId);
            ViewData["TutorId"] = new SelectList(_context.Tutor, "TutorId", "TutorId", tutorType.TutorId);
            return View(tutorType);
        }

        // GET: TutorTypes/Edit/5
        public async Task<IActionResult> Edit(int? id, TutorType model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorType = await _context.TutorType.FindAsync(id);
            model.TutorId = tutorType.TutorId;
            model.InstrumentId = tutorType.InstrumentId;
            model.MaxGrade = tutorType.MaxGrade;
            if (tutorType == null)
            {
                return NotFound();
            }
            List<Tutor> tutors = _context.Tutor.Include(g => g.Staff).ThenInclude(s =>s.Person).ToList();
            List<NameViewModel> tutorNames = tutors.Select(tutor => new NameViewModel
            {
                id = tutor.TutorId,
                name = tutor.Staff.Person.FirstName + " " + tutor.Staff.Person.LastName

            }).ToList();
            ViewData["Tutor"] = new SelectList(tutorNames, "id", "name");

            List<Instrument> instruments = _context.Instrument.ToList();
            List<NameViewModel> instrumentNames = instruments.Select(instrument => new NameViewModel
            {
                id = instrument.InstrumentId,
                name = instrument.Instrument1

            }).ToList();
            ViewData["Instrument"] = new SelectList(instrumentNames, "id", "name");
            return View(model);
        }

        // POST: TutorTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TutorId,InstrumentId,MaxGrade")] TutorType tutorType)
        {
            TutorType tutorT = await _context.TutorType.FindAsync(id);
            tutorT.MaxGrade = tutorType.MaxGrade;
            tutorT.InstrumentId = tutorType.InstrumentId;
            tutorT.Instrument = tutorType.Instrument;
            tutorT.TutorId = tutorType.TutorId;
            tutorT.Tutor = tutorType.Tutor;
            _context.Update(tutorT);


            if (id != tutorT.TutorTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tutorT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorTypeExists(tutorT.TutorTypeId))
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
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1", tutorType.InstrumentId);
            ViewData["TutorId"] = new SelectList(_context.Tutor, "TutorId", "TutorId", tutorType.TutorId);
            return View(tutorType);
        }

        // GET: TutorTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorType = await _context.TutorType
                .Include(t => t.Instrument)
                .Include(t => t.Tutor)
                .ThenInclude(t => t.Staff)
                .ThenInclude(s => s.Person)
                .FirstOrDefaultAsync(m => m.TutorTypeId == id);
            if (tutorType == null)
            {
                return NotFound();
            }

            return View(tutorType);
        }

        // POST: TutorTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutorType = await _context.TutorType.FindAsync(id);
            _context.TutorType.Remove(tutorType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorTypeExists(int id)
        {
            return _context.TutorType.Any(e => e.TutorTypeId == id);
        }
    }
}
