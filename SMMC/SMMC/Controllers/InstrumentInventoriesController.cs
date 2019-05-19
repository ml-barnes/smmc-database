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
    public class InstrumentInventoriesController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public InstrumentInventoriesController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: InstrumentInventories
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.InstrumentInventory.Include(i => i.Instrument);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: InstrumentInventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentInventory = await _context.InstrumentInventory
                .Include(i => i.Instrument)
                .FirstOrDefaultAsync(m => m.InstrumentInventoryId == id);
            if (instrumentInventory == null)
            {
                return NotFound();
            }

            return View(instrumentInventory);
        }

        // GET: InstrumentInventories/Create
        public IActionResult Create()
        {
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1");
            return View();
        }

        // POST: InstrumentInventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddInstrumentViewModel vm)
        {
            int quantity = vm.Quantity; 
            if (ModelState.IsValid)
            {
                for (int i = 0; i < quantity; i++)
                {
                    InstrumentInventory instrumentInventory = new InstrumentInventory();
                    instrumentInventory.InstrumentId = vm.InstrumentId;
                    instrumentInventory.Manufacturer = vm.Manufacturer;
                    instrumentInventory.PurchaseDate = DateTime.Now;
                    instrumentInventory.Cost = vm.Cost;
                    _context.Add(instrumentInventory);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
            {
                var test = error;
            }



            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1", vm.InstrumentId);
            return View(vm);
        }

        // GET: InstrumentInventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentInventory = await _context.InstrumentInventory.FindAsync(id);
            if (instrumentInventory == null)
            {
                return NotFound();
            }
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1", instrumentInventory.InstrumentId);
            return View(instrumentInventory);
        }

        // POST: InstrumentInventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstrumentInventoryId,InstrumentId,PurchaseDate,Manufacturer,Cost")] InstrumentInventory instrumentInventory)
        {
            if (id != instrumentInventory.InstrumentInventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instrumentInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstrumentInventoryExists(instrumentInventory.InstrumentInventoryId))
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
            ViewData["InstrumentId"] = new SelectList(_context.Instrument, "InstrumentId", "Instrument1", instrumentInventory.InstrumentId);
            return View(instrumentInventory);
        }

        // GET: InstrumentInventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instrumentInventory = await _context.InstrumentInventory
                .Include(i => i.Instrument)
                .FirstOrDefaultAsync(m => m.InstrumentInventoryId == id);
            if (instrumentInventory == null)
            {
                return NotFound();
            }

            return View(instrumentInventory);
        }

        // POST: InstrumentInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrumentInventory = await _context.InstrumentInventory.FindAsync(id);
            _context.InstrumentInventory.Remove(instrumentInventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstrumentInventoryExists(int id)
        {
            return _context.InstrumentInventory.Any(e => e.InstrumentInventoryId == id);
        }
    }
}
