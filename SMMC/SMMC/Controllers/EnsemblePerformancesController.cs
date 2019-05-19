using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMMC.Models;

namespace SMMC.Controllers
{
    public class EnsemblePerformancesController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public EnsemblePerformancesController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: EnsemblePerformances
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.EnsemblePerformance.Include(e => e.Ensemble).Include(e => e.Performance).ThenInclude(v => v.Venue);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: EnsemblePerformances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ensemblePerformance = await _context.EnsemblePerformance
                .Include(e => e.Ensemble)
                .Include(e => e.Performance)
                .FirstOrDefaultAsync(m => m.EnsembleId == id);
            if (ensemblePerformance == null)
            {
                return NotFound();
            }

            return View(ensemblePerformance);
        }

        // GET: EnsemblePerformances/Create
        public IActionResult Create()
        {
            ViewData["EnsembleId"] = new SelectList(_context.Ensemble, "EnsembleId", "Type");
            ViewData["PerformanceId"] = new SelectList(_context.Performance, "PerformanceId", "PerformanceId");
            return View();
        }

        // POST: EnsemblePerformances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnsembleId,PerformanceId")] EnsemblePerformance ensemblePerformance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ensemblePerformance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnsembleId"] = new SelectList(_context.Ensemble, "EnsembleId", "Type", ensemblePerformance.EnsembleId);
            ViewData["PerformanceId"] = new SelectList(_context.Performance, "PerformanceId", "PerformanceId", ensemblePerformance.PerformanceId);
            return View(ensemblePerformance);
        }

        // GET: EnsemblePerformances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ensemblePerformance = await _context.EnsemblePerformance.FindAsync(id);
            if (ensemblePerformance == null)
            {
                return NotFound();
            }
            ViewData["EnsembleId"] = new SelectList(_context.Ensemble, "EnsembleId", "Type", ensemblePerformance.EnsembleId);
            ViewData["PerformanceId"] = new SelectList(_context.Performance, "PerformanceId", "PerformanceId", ensemblePerformance.PerformanceId);
            return View(ensemblePerformance);
        }

        // POST: EnsemblePerformances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnsembleId,PerformanceId")] EnsemblePerformance ensemblePerformance)
        {
            if (id != ensemblePerformance.EnsembleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ensemblePerformance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnsemblePerformanceExists(ensemblePerformance.EnsembleId))
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
            ViewData["EnsembleId"] = new SelectList(_context.Ensemble, "EnsembleId", "Type", ensemblePerformance.EnsembleId);
            ViewData["PerformanceId"] = new SelectList(_context.Performance, "PerformanceId", "PerformanceId", ensemblePerformance.PerformanceId);
            return View(ensemblePerformance);
        }

        // GET: EnsemblePerformances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ensemblePerformance = await _context.EnsemblePerformance
                .Include(e => e.Ensemble)
                .Include(e => e.Performance)
                .FirstOrDefaultAsync(m => m.EnsembleId == id);
            if (ensemblePerformance == null)
            {
                return NotFound();
            }

            return View(ensemblePerformance);
        }

        // POST: EnsemblePerformances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ensemblePerformance = await _context.EnsemblePerformance.FindAsync(id);
            _context.EnsemblePerformance.Remove(ensemblePerformance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnsemblePerformanceExists(int id)
        {
            return _context.EnsemblePerformance.Any(e => e.EnsembleId == id);
        }
    }
}
