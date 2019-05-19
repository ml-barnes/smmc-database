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
    public class PerformancesController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public PerformancesController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Performances
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.Performance.Include(p => p.Venue);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Performances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance
                .Include(p => p.Venue)
                .Include(p => p.EnsemblePerformance)
                    .ThenInclude(ep => ep.Ensemble)
                .Include(p=>p.PerformancePiece)                    
                .FirstOrDefaultAsync(m => m.PerformanceId == id);

            var ps = _context.PerformancePiece.Include(pp=>pp.Piece).Where(pp => pp.PerformanceId == id).ToList();
            ViewData["Pieces"] = ps;


            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // GET: Performances/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueId");
            List<Venue> venueNames = _context.Venue.ToList();
            ViewData["Venue"] = new SelectList(venueNames, "VenueId", "VenueName");
            return View();
        }

        // POST: Performances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PerformanceId,Date,Time,VenueId")] Performance performance)
        {
            _context.Performance.Add(performance);
            await _context.SaveChangesAsync();

            List<Ensemble> ensembles = _context.Ensemble.ToList();
            foreach (var item in ensembles)
            {
                //Person
                int id = item.EnsembleId;
                EnsemblePerformance ensemblePerformance = new EnsemblePerformance();
                ensemblePerformance.EnsembleId = id;
                ensemblePerformance.Ensemble = await _context.Ensemble.FindAsync(id);
                ensemblePerformance.PerformanceId = performance.PerformanceId;
                

                _context.Add(ensemblePerformance);
                await _context.SaveChangesAsync();


            }
            return RedirectToAction("Details", new { id = performance.PerformanceId });
        }

        // GET: Performances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance.FindAsync(id);
            if (performance == null)
            {
                return NotFound();
            }
            List<Venue> venueNames = _context.Venue.ToList();
            ViewData["Venue"] = new SelectList(venueNames, "VenueId", "VenueName");
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueId", performance.VenueId);
            return View(performance);
        }

        // POST: Performances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PerformanceId,Date,Time,VenueId")] Performance performance)
        {
            if (id != performance.PerformanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceExists(performance.PerformanceId))
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
            ViewData["VenueId"] = new SelectList(_context.Venue, "VenueId", "VenueId", performance.VenueId);
            return View(performance);
        }

        // GET: Performances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performance = await _context.Performance
                .Include(p => p.Venue)
                .FirstOrDefaultAsync(m => m.PerformanceId == id);
            if (performance == null)
            {
                return NotFound();
            }

            return View(performance);
        }

        // POST: Performances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performance = await _context.Performance.FindAsync(id);
            _context.Performance.Remove(performance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerformanceExists(int id)
        {
            return _context.Performance.Any(e => e.PerformanceId == id);
        }
    }
}
