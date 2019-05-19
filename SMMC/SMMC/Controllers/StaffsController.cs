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
    public class StaffsController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public StaffsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.Staff.Include(s => s.Contact).Include(s => s.Person);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.Contact)
                .Include(s => s.Person)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Email");
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "Address");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StaffId,PersonId,ContactId,StartDate,LeftDate,Hours")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Email", staff.ContactId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "Address", staff.PersonId);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id, StaffViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.Person)
                .Include(s => s.Contact)
                .FirstOrDefaultAsync(m => m.StaffId == id);

            model.FirstName = staff.Person.FirstName;
            model.LastName = staff.Person.LastName;
            model.Dob = staff.Person.Dob;
            model.Address = staff.Person.Address;

            model.Email = staff.Contact.Email;
            model.Phone = staff.Contact.Phone;
            model.Hours = staff.Hours;
            model.StartDate = staff.StartDate;
            if (staff.LeftDate != null)
            {
                model.LeftDate = staff.LeftDate;
            }

            if (staff == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StaffViewModel model)
        {
            var staff = await _context.Staff
                .Include(s => s.Person)
                .Include(s => s.Contact)
                .FirstOrDefaultAsync(m => m.StaffId == id);

            staff.Person.FirstName = model.FirstName;
            staff.Person.LastName = model.LastName;
            staff.Person.Dob = model.Dob;
            staff.Person.Address = model.Address;
            staff.Contact.Email = model.Email;
            staff.Contact.Phone = model.Phone;
            staff.Hours = model.Hours;
            staff.StartDate = model.StartDate;
            if(model.LeftDate != null)
            {
                staff.LeftDate = model.LeftDate;
            }
            _context.Update(staff);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = staff.StaffId });
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.Contact)
                .Include(s => s.Person)
                .FirstOrDefaultAsync(m => m.StaffId == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.StaffId == id);
        }
    }
}
