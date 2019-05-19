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
    public class SheetMusicsController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public SheetMusicsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: SheetMusics
        public async Task<IActionResult> Index()
        {
            return View(await _context.SheetMusic.ToListAsync());
        }

        // GET: SheetMusics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheetMusic = await _context.SheetMusic
                .FirstOrDefaultAsync(m => m.SheetMusicId == id);
            if (sheetMusic == null)
            {
                return NotFound();
            }

            return View(sheetMusic);
        }

        // GET: SheetMusics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SheetMusics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SheetMusicId,Title,Composer,Amount")] SheetMusic sheetMusic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sheetMusic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sheetMusic);
        }

        // GET: SheetMusics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheetMusic = await _context.SheetMusic.FindAsync(id);
            if (sheetMusic == null)
            {
                return NotFound();
            }
            return View(sheetMusic);
        }

        // POST: SheetMusics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SheetMusicId,Title,Composer,Amount")] SheetMusic sm)
        {
            SheetMusic sheetMusic = await _context.SheetMusic.FindAsync(id);
            sheetMusic.Amount = sm.Amount;
            if (id != sheetMusic.SheetMusicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sheetMusic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SheetMusicExists(sheetMusic.SheetMusicId))
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
            return View(sheetMusic);
        }

        // GET: SheetMusics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheetMusic = await _context.SheetMusic
                .FirstOrDefaultAsync(m => m.SheetMusicId == id);
            if (sheetMusic == null)
            {
                return NotFound();
            }

            return View(sheetMusic);
        }

        // POST: SheetMusics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sheetMusic = await _context.SheetMusic.FindAsync(id);
            _context.SheetMusic.Remove(sheetMusic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SheetMusicExists(int id)
        {
            return _context.SheetMusic.Any(e => e.SheetMusicId == id);
        }
    }
}
