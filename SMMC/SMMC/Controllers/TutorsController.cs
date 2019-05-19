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
    public class TutorsController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public TutorsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Tutors
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.Tutor.Include(t => t.Staff).ThenInclude(s => s.Person).Include(t => t.Staff).ThenInclude(c => c.Contact);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Tutors/Details/5
        public async Task<IActionResult> Details(int? id, TutorCreateViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            Tutor tut = await _context.Tutor
                .Include(t=>t.Staff)
                    .ThenInclude(a=>a.Person)
                 .Include(b=>b.Staff).ThenInclude(c=>c.Contact)
                .FirstAsync(m => m.TutorId == id); 
            
            model.FirstName = tut.Staff.Person.FirstName;
            model.LastName = tut.Staff.Person.LastName;
            model.Address = tut.Staff.Person.Address;
            model.Email = tut.Staff.Contact.Email;
            model.Phone = tut.Staff.Contact.Phone;
            model.Hours = tut.Staff.Hours;

            var canteach = _context.TutorType
                .Include(tt => tt.Tutor)
                .ThenInclude(t => t.Staff)
                    .ThenInclude(s => s.Contact)
                .ThenInclude(tt => tt.Staff)
                    .ThenInclude(s => s.Person)
                .Include(tt => tt.Instrument)
                .Where(m => m.TutorId == id).ToList();
            var details = canteach.FirstOrDefault();

            model.InstrumentsCanTeach = new List<string>();
            foreach (var item in canteach)
            {
                Instrument instrument = await _context.Instrument.FirstOrDefaultAsync(e => e.InstrumentId == item.InstrumentId);
                model.InstrumentsCanTeach.Add(instrument.Instrument1);
            }

            var teaching = await _context.EnrollmentMusicClass
                .Include(emc => emc.MusicClass)
                .Include(emc => emc.Enrollment)
                    .ThenInclude(e => e.Instrument)
                    .FirstOrDefaultAsync(mc => mc.MusicClass.TutorId == id);

            if(teaching != null)
            {
                model.InstrumentsTeaching = new List<string>();
                foreach (var item in teaching.MusicClass.EnrollmentMusicClass)
                {
                    Instrument instrument = await _context.Instrument.FirstOrDefaultAsync(e => e.InstrumentId == item.Enrollment.Instrument.InstrumentId);
                    model.InstrumentsTeaching.Add(instrument.Instrument1);
                }
            }            

            return View(model);
        }

        // GET: Tutors/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staff, "StaffId", "StaffId");
            return View();
        }

        // POST: Tutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TutorCreateViewModel model)
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
            staff.LeftDate = new DateTime(04/07/2020);
            staff.Hours = model.Hours;

            _context.Staff.Add(staff);
            _context.SaveChanges();

            int latestStaff = staff.StaffId;

            //Tutor

            Tutor tutor = new Tutor();
            tutor.StaffId = latestStaff;
            tutor.Atcl = model.Atcl;

            _context.Tutor.Add(tutor);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = tutor.TutorId });
        }

        // GET: Tutors/Edit/5
        public async Task<IActionResult> Edit(int? id, TutorCreateViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutor
                .Include(t => t.Staff)
                .ThenInclude(s => s.Person)
                .Include(t => t.Staff)
                .ThenInclude(s => s.Contact)
                .FirstOrDefaultAsync(m => m.TutorId == id);

            model.FirstName = tutor.Staff.Person.FirstName;
            model.LastName = tutor.Staff.Person.LastName;
            model.Dob = tutor.Staff.Person.Dob;
            model.Address = tutor.Staff.Person.Address;

            model.Email = tutor.Staff.Contact.Email;
            model.Phone = tutor.Staff.Contact.Phone;
            model.Hours = tutor.Staff.Hours;

            return View(model);
        }

        // POST: Tutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TutorCreateViewModel model)
        {
            var tutor = await _context.Tutor
                .Include(t => t.Staff)
                .ThenInclude(s => s.Person)
                .Include(t => t.Staff)
                .ThenInclude(s => s.Contact)
                .FirstOrDefaultAsync(m => m.TutorId == id);

            tutor.Staff.Person.FirstName = model.FirstName;
            tutor.Staff.Person.LastName = model.LastName;
            tutor.Staff.Person.Dob = model.Dob;
            tutor.Staff.Person.Address = model.Address;
            tutor.Staff.Contact.Email = model.Email;
            tutor.Staff.Contact.Phone = model.Phone;
            tutor.Staff.Hours = model.Hours;
            _context.Update(tutor);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = tutor.TutorId });
        }

        // GET: Tutors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutor
                .Include(t => t.Staff)
                .ThenInclude(s => s.Person)
                .FirstOrDefaultAsync(m => m.TutorId == id);
            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // POST: Tutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutor = await _context.Tutor.FindAsync(id);
            _context.Tutor.Remove(tutor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorExists(int id)
        {
            return _context.Tutor.Any(e => e.TutorId == id);
        }
    }
}
