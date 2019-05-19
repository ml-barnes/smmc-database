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
    public class PayrollsController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public PayrollsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Payrolls
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.Payroll.Include(p => p.Staff).ThenInclude(s => s.Person);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        public IActionResult GeneratePayroll()
        {
            string proc = "[paypeople]";
            var loanResult = _context.Database.ExecuteSqlCommand(proc);

            return RedirectToAction("Index");
        }

        // GET: Payrolls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payroll
                .Include(p => p.Staff)
                .ThenInclude(s => s.Person)
                .FirstOrDefaultAsync(m => m.PayrollId == id);
            if (payroll == null)
            {
                return NotFound();
            }

            return View(payroll);
        }

        // GET: Payrolls/Create
        public IActionResult Create()
        {
            List<Staff> staffs = _context.Staff.Include(e => e.Person).ToList();
            List<StaffPayViewModel> staffNames = staffs.Select(staff => new StaffPayViewModel
            {
                id = staff.StaffId,
                name = staff.Person.FirstName + " " + staff.Person.LastName

            }).ToList();
            ViewData["Staff"] = new SelectList(staffNames, "id", "name");
            return View();
        }

        // POST: Payrolls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PayrollId,StaffId,Date,Amount,Notes")] Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payroll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", payroll.StaffId);
            return View(payroll);
        }

        // GET: Payrolls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payroll.FindAsync(id);
            if (payroll == null)
            {
                return NotFound();
            }
            List<Staff> staffs = _context.Staff.Include(s => s.Person).ToList();
            List<NameViewModel> staffNames = staffs.Select(staff => new NameViewModel
            {
                id = staff.StaffId,
                name = staff.Person.FirstName + " " + staff.Person.LastName

            }).ToList();
            ViewData["Staff"] = new SelectList(staffNames, "id", "name");
            return View(payroll);
        }

        // POST: Payrolls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayrollId,StaffId,Date,Amount,Notes")] Payroll payroll)
        {
            if (id != payroll.PayrollId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payroll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayrollExists(payroll.PayrollId))
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
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId", payroll.StaffId);
            return View(payroll);
        }

        // GET: Payrolls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payroll = await _context.Payroll
                .Include(p => p.Staff)
                .ThenInclude(s => s.Person)
                .FirstOrDefaultAsync(m => m.PayrollId == id);
            if (payroll == null)
            {
                return NotFound();
            }

            return View(payroll);
        }

        // POST: Payrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payroll = await _context.Payroll.FindAsync(id);
            _context.Payroll.Remove(payroll);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayrollExists(int id)
        {
            return _context.Payroll.Any(e => e.PayrollId == id);
        }
    }
}
