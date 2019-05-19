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
    public class TechniciansController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public TechniciansController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Technicians
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.Technician.Include(t => t.Staff).ThenInclude(s => s.Person).Include(t => t.Staff).ThenInclude(c => c.Contact);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Technicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technician
                .Include(t => t.Staff)
                .ThenInclude(s => s.Contact)
                .Include(t => t.Staff)
                .ThenInclude(s => s.Person)
                .FirstOrDefaultAsync(m => m.TechnicianId == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // GET: Technicians/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId");
            return View();
        }

        // POST: Technicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TechnicianCreateViewModel model)
        {
            //Person
            Person person = new Person();
            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.Dob = model.Dob;
            person.Address = model.Address;

            _context.Person.Add(person);
            _context.SaveChanges();

            int latestPerson = person.PersonId;

            //Contact
            Contact contact = new Contact();
            contact.Email = model.Email;
            contact.Phone = model.Phone;

            _context.Contact.Add(contact);
            _context.SaveChanges();

            int latestContact = contact.ContactId;

            //Staff
            Staff staff = new Staff();
            staff.PersonId = latestPerson;
            staff.ContactId = latestContact;
            staff.StartDate = model.StartDate;
            //Change to null
            staff.LeftDate = new DateTime(04 / 07 / 2020);
            staff.Hours = model.Hours;

            _context.Staff.Add(staff);
            _context.SaveChanges();

            int latestStaff = staff.StaffId;

            //Tutor

            Technician technician = new Technician();
            technician.StaffId = latestStaff;

            _context.Technician.Add(technician);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = technician.TechnicianId });
        }

        // GET: Technicians/Edit/5
        public async Task<IActionResult> Edit(int? id, TechnicianCreateViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tech = await _context.Technician
                .Include(t => t.Staff)
                .ThenInclude(s => s.Person)
                .Include(t => t.Staff)
                .ThenInclude(s => s.Contact)
                .FirstOrDefaultAsync(m => m.TechnicianId == id);

            model.FirstName = tech.Staff.Person.FirstName;
            model.LastName = tech.Staff.Person.LastName;
            model.Dob = tech.Staff.Person.Dob;
            model.Address = tech.Staff.Person.Address;

            model.Email = tech.Staff.Contact.Email;
            model.Phone = tech.Staff.Contact.Phone;
            model.Hours = tech.Staff.Hours;

            return View(model);
        }

        // POST: Technicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TechnicianCreateViewModel model)
        {
            var tech = await _context.Technician
                .Include(t => t.Staff)
                .ThenInclude(s => s.Person)
                .Include(t => t.Staff)
                .ThenInclude(s => s.Contact)
                .FirstOrDefaultAsync(m => m.TechnicianId == id);

            tech.Staff.Person.FirstName = model.FirstName;
            tech.Staff.Person.LastName = model.LastName;
            tech.Staff.Person.Dob = model.Dob;
            tech.Staff.Person.Address = model.Address;
            tech.Staff.Contact.Email = model.Email;
            tech.Staff.Contact.Phone = model.Phone;
            tech.Staff.Hours = model.Hours;
            _context.Update(tech);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = tech.TechnicianId });
        }

        // GET: Technicians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technician
                .Include(t => t.Staff)
                .ThenInclude(s => s.Person)
                .FirstOrDefaultAsync(m => m.TechnicianId == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // POST: Technicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var technician = await _context.Technician.FindAsync(id);
            _context.Technician.Remove(technician);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechnicianExists(int id)
        {
            return _context.Technician.Any(e => e.TechnicianId == id);
        }
    }
}
