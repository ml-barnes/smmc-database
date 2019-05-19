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
    public class EnsemblesController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public EnsemblesController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Ensembles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ensemble.ToListAsync());
        }

        // GET: Ensembles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ensemble = await _context.Ensemble
                .FirstOrDefaultAsync(m => m.EnsembleId == id);
            if (ensemble == null)
            {
                return NotFound();
            }

            return View(ensemble);
        }

        // GET: Ensembles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ensembles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnsembleId,Type")] Ensemble ensemble)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ensemble);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ensemble);
        }

        // GET: Ensembles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ensemble = await _context.Ensemble.FindAsync(id);
            if (ensemble == null)
            {
                return NotFound();
            }
            return View(ensemble);
        }

        // POST: Ensembles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnsembleId,Type")] Ensemble ensemble)
        {
            if (id != ensemble.EnsembleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ensemble);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnsembleExists(ensemble.EnsembleId))
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
            return View(ensemble);
        }

        // GET: Ensembles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ensemble = await _context.Ensemble
                .FirstOrDefaultAsync(m => m.EnsembleId == id);
            if (ensemble == null)
            {
                return NotFound();
            }

            return View(ensemble);
        }

        // POST: Ensembles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ensemble = await _context.Ensemble.FindAsync(id);
            _context.Ensemble.Remove(ensemble);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnsembleExists(int id)
        {
            return _context.Ensemble.Any(e => e.EnsembleId == id);
        }
    }
}
