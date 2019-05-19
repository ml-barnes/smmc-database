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
    public class StudentsController : Controller
    {
        private const int MIN_AGE = 5;
        private readonly IN705_2018S2_db3shared01Context _context;

        public StudentsController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.Student.Include(s => s.Contact).Include(s => s.Person);
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id, StudentCreateViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Person)
                .Include(s => s.Contact)
                .Include(s => s.Enrollment)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            model.FirstName = student.Person.FirstName;
            model.LastName = student.Person.LastName;
            model.Dob = student.Person.Dob;
            model.Address = student.Person.Address;
            if(student.Contact != null)
            {
                model.Phone = student.Contact.Phone;
                model.Email = student.Contact.Email;
            }
            model.Enrollments = new List<string>();
            foreach(var item in student.Enrollment)
            {
                Instrument instrument = await _context.Instrument.FirstOrDefaultAsync(e => e.InstrumentId == item.InstrumentId);
                model.Enrollments.Add(instrument.Instrument1 + ": Grade " + item.Grade);
            }

            if (student == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Students/Create
        public IActionResult Create(String DateError = null)
        {
            if(DateError != null)
            {
                ViewData["Error"] = DateError;
            }
            List<Guardian> guardians = _context.Guardian.Include(g => g.GuardianNavigation).ToList();
            List<NameViewModel> guardianNames = guardians.Select(guardian => new NameViewModel
            {
                id = guardian.GuardianId,
                name = guardian.GuardianNavigation.FirstName + " " + guardian.GuardianNavigation.LastName

            }).ToList();
            ViewData["Guardian"] = new SelectList(guardianNames, "id", "name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateViewModel model)
        {
            if (model.Dob.Year > (DateTime.Now.Year - MIN_AGE))
            {
                return RedirectToAction("Create", new { DateError = "Student must be older than 5"});
            }
            Person person = new Person();
            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.Dob = model.Dob;
            person.Address = model.Address;

            _context.Person.Add(person);
            _context.SaveChanges();

            int latestPerson = person.PersonId;

            Student student = new Student();
            student.PersonId = latestPerson;
            if (model.Email != null)
            {
                Contact contact = new Contact();
                contact.Email = model.Email;
                contact.Phone = model.Phone;

                _context.Contact.Add(contact);
                _context.SaveChanges();

                int latestContact = contact.ContactId;
                student.ContactId = latestContact;
            }

            _context.Student.Add(student);
            _context.SaveChanges();

            int latestStudent = student.StudentId;

            if (model.GuardianFirstName != null)
            {
                Contact contact = new Contact();
                contact.Email = model.GuardianEmail;
                contact.Phone = model.GuardianPhone;

                _context.Contact.Add(contact);
                _context.SaveChanges();

                int latestContact = contact.ContactId;

                Person guardianPerson = new Person();
                guardianPerson.FirstName = model.GuardianFirstName;
                guardianPerson.LastName = model.GuardianLastName;
                guardianPerson.Dob = model.GuardianDob;
                guardianPerson.Address = model.GuardianAddress;

                _context.Person.Add(guardianPerson);
                _context.SaveChanges();

                int latestGuardianPerson = guardianPerson.PersonId;

                Guardian guardian = new Guardian();
                guardian.ContactId = latestContact;
                guardian.PersonId = latestGuardianPerson;

                _context.Guardian.Add(guardian);
                _context.SaveChanges();

                int latestGuardian = guardian.GuardianId;

                StudentGuardian studentGuardian = new StudentGuardian();
                studentGuardian.StudentId = latestStudent;
                studentGuardian.GuardianId = latestGuardian;

                _context.StudentGuardian.Add(studentGuardian);
                _context.SaveChanges();
            }
            else if(model.guardian)
            {
                string id = Request.Form["GuardianDropDown"];
                StudentGuardian studentGuardian = new StudentGuardian();
                studentGuardian.StudentId = latestStudent;
                studentGuardian.GuardianId = Convert.ToInt32(id);

                _context.StudentGuardian.Add(studentGuardian);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = student.StudentId });
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id, StudentEditViewModel model)
        {
            

            var student = await _context.Student
                .Include(s => s.Person)
                .Include(s => s.Contact)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            model.StudentId = student.StudentId;
            model.FirstName = student.Person.FirstName;
            model.LastName = student.Person.LastName;
            model.Dob = student.Person.Dob;
            model.Address = student.Person.Address;

            if(student.Contact != null)
            {
                model.Email = student.Contact.Email;
                model.Phone = student.Contact.Phone;
            }

            if (student == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentEditViewModel model)
        {
            var student = await _context.Student
                .Include(s => s.Person)
                .Include(s => s.Contact)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            student.Person.FirstName = model.FirstName;
            student.Person.LastName = model.LastName;
            student.Person.Dob = model.Dob;
            student.Person.Address = model.Address;
            if (model.Email != null)
            {
                if (student.ContactId == null)
                {
                    Contact contact = new Contact();
                    contact.Email = model.Email;
                    contact.Phone = model.Phone;

                    _context.Contact.Add(contact);
                    _context.SaveChanges();

                    student.Contact = contact;
                }
                else
                {
                    student.Contact.Email = model.Email;
                    student.Contact.Phone = model.Phone;
                }
            }
            _context.Update(student);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = student.StudentId });
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Contact)
                .Include(s => s.Person)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
