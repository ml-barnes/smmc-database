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
    public class LocalMusiciansController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;

        public LocalMusiciansController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        // GET: LocalMusicians
        public async Task<IActionResult> Index()
        {
            var iN705_2018S2_db3shared01Context = _context.LocalMusicians.Include(l => l.Contact);            
            return View(await iN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: LocalMusicians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localMusicians = await _context.LocalMusicians
                .Include(l => l.Contact)
                .FirstOrDefaultAsync(m => m.LocalMusiciansId == id);
            if (localMusicians == null)
            {
                return NotFound();
            }

            return View(localMusicians);
        }

        // GET: LocalMusicians/Create
        public IActionResult Create()
        {
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Email");
            return View();
        }

        // POST: LocalMusicians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TutorCreateViewModel model)
        {
            //Contact
            Contact contact = new Contact();
            contact.Email = model.Email;
            contact.Phone = model.Phone;

            _context.Contact.Add(contact);
            _context.SaveChanges();

            int latestContact = contact.ContactId;

            LocalMusicians localMusicians = new LocalMusicians();
            localMusicians.ContactId = latestContact;
            localMusicians.FirstName = model.FirstName;
            localMusicians.LastName = model.LastName;

            _context.LocalMusicians.Add(localMusicians);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = localMusicians.LocalMusiciansId });
        }
        public async Task<IActionResult> AddToEnsemble(int id)
        {
            var localMusician = await _context.LocalMusicians
                 .FirstOrDefaultAsync(m => m.LocalMusiciansId == id);

            if (localMusician == null)
            {
                return NotFound();
            }

            return View(localMusician);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAddToEnsemble()
        {
            LocalMusiciansEnsemble lme = new LocalMusiciansEnsemble();
            lme.EnsembleId = 2;
            var id = Request.Form["LocalMusiciansId"];
            lme.LocalMusiciansId = Convert.ToInt32(id);
            _context.LocalMusiciansEnsemble.Add(lme);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = lme.LocalMusiciansId });
        }

        // GET: LocalMusicians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localMusicians = await _context.LocalMusicians.FindAsync(id);
            if (localMusicians == null)
            {
                return NotFound();
            }
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Email", localMusicians.ContactId);
            return View(localMusicians);
        }

        // POST: LocalMusicians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocalMusiciansId,ContactId,FirstName,LastName")] LocalMusicians localMusicians)
        {
            if (id != localMusicians.LocalMusiciansId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localMusicians);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalMusiciansExists(localMusicians.LocalMusiciansId))
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
            ViewData["ContactId"] = new SelectList(_context.Contact, "ContactId", "Email", localMusicians.ContactId);
            return View(localMusicians);
        }

        // GET: LocalMusicians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localMusicians = await _context.LocalMusicians
                .Include(l => l.Contact)
                .FirstOrDefaultAsync(m => m.LocalMusiciansId == id);
            if (localMusicians == null)
            {
                return NotFound();
            }

            return View(localMusicians);
        }

        // POST: LocalMusicians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var localMusicians = await _context.LocalMusicians.FindAsync(id);
            _context.LocalMusicians.Remove(localMusicians);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalMusiciansExists(int id)
        {
            return _context.LocalMusicians.Any(e => e.LocalMusiciansId == id);
        }
    }
}
