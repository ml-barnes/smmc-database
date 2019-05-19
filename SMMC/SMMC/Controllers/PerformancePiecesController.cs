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
    public class PerformancePiecesController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public PerformancePiecesController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: PerformancePieces
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.PerformancePiece.Include(p => p.Performance).Include(p => p.Piece);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: PerformancePieces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performancePiece = await _context.PerformancePiece
                .Include(p => p.Performance)
                .Include(p => p.Piece)
                .FirstOrDefaultAsync(m => m.PerformancePieceId == id);
            if (performancePiece == null)
            {
                return NotFound();
            }

            return View(performancePiece);
        }

        // GET: PerformancePieces/Create
        public IActionResult Create(int id)
        {
            ViewData["PerformanceId"] = id;

            List<Piece> pieces = _context.Piece.FromSql("[Get Available Pieces] " + id.ToString() ).ToList();

            if (pieces.Count != 0)
            {
                ViewData["PieceId"] = new SelectList(pieces, "PieceId", "Title");
            }
            
            return View();
        }

        // POST: PerformancePieces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerformancePiece performancePiece)
        {
            Piece piece = await _context.Piece.FindAsync(performancePiece.PieceId);
            piece.LastPerformedDate = DateTime.Now;
            _context.Update(piece);
            await _context.SaveChangesAsync();
            _context.Add(performancePiece);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Performances");
        }

        // GET: PerformancePieces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performancePiece = await _context.PerformancePiece.FindAsync(id);
            if (performancePiece == null)
            {
                return NotFound();
            }
            ViewData["PerformanceId"] = new SelectList(_context.Performance, "PerformanceId", "PerformanceId", performancePiece.PerformanceId);
            ViewData["PieceId"] = new SelectList(_context.Piece, "PieceId", "PieceId", performancePiece.PieceId);
            return View(performancePiece);
        }

        // POST: PerformancePieces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PerformancePieceId,PerformanceId,PieceId")] PerformancePiece performancePiece)
        {
            if (id != performancePiece.PerformancePieceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performancePiece);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformancePieceExists(performancePiece.PerformancePieceId))
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
            ViewData["PerformanceId"] = new SelectList(_context.Performance, "PerformanceId", "PerformanceId", performancePiece.PerformanceId);
            ViewData["PieceId"] = new SelectList(_context.Piece, "PieceId", "PieceId", performancePiece.PieceId);
            return View(performancePiece);
        }

        // GET: PerformancePieces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performancePiece = await _context.PerformancePiece
                .Include(p => p.Performance)
                .Include(p => p.Piece)
                .FirstOrDefaultAsync(m => m.PerformancePieceId == id);
            if (performancePiece == null)
            {
                return NotFound();
            }

            return View(performancePiece);
        }

        // POST: PerformancePieces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performancePiece = await _context.PerformancePiece.FindAsync(id);
            Piece piece = await _context.Piece.FirstOrDefaultAsync(p => p.PieceId == performancePiece.PieceId);
            piece.LastPerformedDate = null;
            _context.Update(piece);
            _context.PerformancePiece.Remove(performancePiece);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Performances");
        }

        private bool PerformancePieceExists(int id)
        {
            return _context.PerformancePiece.Any(e => e.PerformancePieceId == id);
        }
    }
}
