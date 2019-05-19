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
    public class RepairsController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public RepairsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Repairs
        public async Task<IActionResult> Index()
        {
            var IN705_2018S2_db3shared01Context = _context.Repairs.Include(r => r.InstrumentInventory.Instrument).Include(r => r.Technician).ThenInclude(a=>a.Staff.Person);
            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        public async Task<IActionResult> Complete()
        {
            var IN705_2018S2_db3shared01Context = _context.Repairs.Include(r => r.InstrumentInventory.Instrument).Include(r => r.Technician).ThenInclude(a => a.Staff.Person);
            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Repairs/Details/5
        public async Task<IActionResult> Details(int id1, int id2, DateTime start, DateTime end)
        {
            var repairs = await _context.Repairs
                .Include(r => r.InstrumentInventory)
                .Include(r => r.Technician)
                .FirstAsync(m => m.TechnicianId == id1 && m.InstrumentInventoryId == id2 && m.RepairEnd == end && m.RepairStart == start);
            if (repairs == null)
            {
                return NotFound();
            }
           
            return View(repairs);
        }

        // GET: Repairs/Create
        public IActionResult Create()
        {
            ViewData["InstrumentInventoryId"] = new SelectList(_context.InstrumentInventory, "InstrumentInventoryId", "Manufacturer");
            ViewData["TechnicianId"] = new SelectList(_context.Technician, "TechnicianId", "TechnicianId");
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentInventoryId,TechnicianId,RepairStart,RepairEnd")] Repairs repairs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repairs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstrumentInventoryId"] = new SelectList(_context.InstrumentInventory, "InstrumentInventoryId", "Manufacturer", repairs.InstrumentInventoryId);
            ViewData["TechnicianId"] = new SelectList(_context.Technician, "TechnicianId", "TechnicianId", repairs.TechnicianId);
            return View(repairs);
        }

        // GET: Repairs/Edit/5
        public async Task<IActionResult> Edit(int id1, int id2)
        {           

            var repairs = await _context.Repairs
                .Include(r=>r.InstrumentInventory)
                .Include(r=>r.Technician)
                .FirstAsync(m => m.TechnicianId == id1 && m.InstrumentInventoryId == id2 && m.RepairEnd == null);
            if (repairs == null)
            {
                return NotFound();
            }
            ViewData["InstrumentInventoryId"] = new SelectList(_context.InstrumentInventory, "InstrumentInventoryId", "Manufacturer", repairs.InstrumentInventoryId);
            ViewData["TechnicianId"] = new SelectList(_context.Technician, "TechnicianId", "TechnicianId", repairs.TechnicianId);
            return View(repairs);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Repairs repairToClose)
        {
            var repairs = await _context.Repairs
                .Include(r => r.InstrumentInventory)
                .Include(r => r.Technician)
                .FirstAsync(m => m.TechnicianId == repairToClose.TechnicianId 
                && m.InstrumentInventoryId == repairToClose.InstrumentInventoryId 
                && m.RepairEnd == null);

            repairs.RepairEnd = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repairs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairsExists(repairs.InstrumentInventoryId))
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
            ViewData["InstrumentInventoryId"] = new SelectList(_context.InstrumentInventory, "InstrumentInventoryId", "Manufacturer", repairs.InstrumentInventoryId);
            ViewData["TechnicianId"] = new SelectList(_context.Technician, "TechnicianId", "TechnicianId", repairs.TechnicianId);
            return View(repairs);
        }

        // GET: Repairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairs = await _context.Repairs
                .Include(r => r.InstrumentInventory)
                .Include(r => r.Technician)
                .FirstOrDefaultAsync(m => m.InstrumentInventoryId == id);
            if (repairs == null)
            {
                return NotFound();
            }

            return View(repairs);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repairs = await _context.Repairs.FindAsync(id);
            _context.Repairs.Remove(repairs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepairsExists(int id)
        {
            return _context.Repairs.Any(e => e.InstrumentInventoryId == id);
        }
    }
}
