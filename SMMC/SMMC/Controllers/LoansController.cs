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
    

    public class LoansController : Controller
    {
        private readonly IN705_2018S2_db3shared01Context _context;
        public string LOAN_ERROR = "No available instruments to loan. Add Instrument to Inventory and try again.";
        public string LOAN_SUCCESS = "Loan Logged successfully.";
        public string LOAN_RETURN_SUCCESS = "Loan Return Logged successfully.";
        public string REPAIR_SUCCESS = "Instrument logged for repair and replacement given.";
        public string REPAIR_ERROR = "Error logging repair.";

        public LoansController(IN705_2018S2_db3shared01Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Feedback(string feedback, string color)
        {
            ViewData["Feedback"] = feedback;
            ViewData["Color"] = color;
            var IN705_2018S2_db3shared01Context = _context.Loan
                .Include(l => l.Enrollment)
                .ThenInclude(s => s.Student.Person)
                .Include(l => l.InstrumentInventory)
                .ThenInclude(a => a.Instrument);

            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            var IN705_2018S2_db3shared01Context = _context.Loan
                .Include(l => l.Enrollment)
                .ThenInclude(s => s.Student.Person)
                .Include(l => l.InstrumentInventory)
                .ThenInclude(a => a.Instrument);
            return View(await IN705_2018S2_db3shared01Context.ToListAsync());
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(int id1, int id2)
        {

            var loan = await _context.Loan
                .Include(l => l.Enrollment)
                .Include(l => l.InstrumentInventory)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id1 && m.InstrumentInventoryId == id2);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            List<Student> students = _context.Student.Include(e => e.Person).ToList();

            List<StudentNameViewModel> studentName = students.Select(student => new StudentNameViewModel
            {
                id = student.StudentId,
                name = student.Person.FirstName + " " + student.Person.LastName

            }).ToList();

            ViewData["Student"] = new SelectList(studentName, "id", "name");    

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectEnrollment(StudentNameViewModel student)
        {  
            var enrollments =  _context.Enrollment.Where(e => e.StudentId == student.id);
            List<EnrollmentDisplayViewModel> ens = enrollments.Select(e => new EnrollmentDisplayViewModel
            {
                id = e.EnrollmentId,
                enrollment = e.Instrument.Instrument1 + " (Grade " + e.Grade + ")"

            }).ToList();

            ViewData["Enrollment"] = new SelectList(ens, "id",  "enrollment");              
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisplayInstruments(EnrollmentDisplayViewModel enrollment)
        {
            string proc = "[Loan Instrument] " + enrollment.id.ToString();
            var loanResult = _context.Database.ExecuteSqlCommand(proc);

            string feedbackString = LOAN_SUCCESS;
            string colorString = "green";

            if (loanResult < 1)
            {
                feedbackString = LOAN_ERROR;
                colorString = "red";
            }
            return RedirectToAction("Feedback", new { feedback = feedbackString, color = colorString });
        }

        public async Task<IActionResult> Repair(int id1, int id2)
        {
            var loan = await _context.Loan
                 .Include(l => l.Enrollment)
                 .Include(l => l.InstrumentInventory)
                 .FirstOrDefaultAsync(m => m.EnrollmentId == id1 &&
                                     m.InstrumentInventoryId == id2 &&
                                     m.DateReturned == null);

            ViewData["EnrollmentId"] = loan.EnrollmentId;
            ViewData["InstrumentInventoryId"] = loan.InstrumentInventoryId;

            if (loan == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Repair(RepairViewModel repairLoan)
        {            
            string insertToRepairs = "Insert into Repairs values (" + repairLoan.InstrumentInventoryId.ToString() + ", (select top 1 [Technician ID] from Technician), GETDATE(), '"  + repairLoan.Notes + "' , null)";
            var repairResult = _context.Database.ExecuteSqlCommand(insertToRepairs);

            //Give out new loan
            string proc = "[Loan Instrument] " + repairLoan.EnrollmentId.ToString();
            var loanResult = _context.Database.ExecuteSqlCommand(proc);
            

            //Return loan
            var loan = await _context.Loan
                .Include(l => l.Enrollment)
                .Include(l => l.InstrumentInventory)
                .FirstOrDefaultAsync(m => m.EnrollmentId == repairLoan.EnrollmentId &&
                                    m.InstrumentInventoryId == repairLoan.InstrumentInventoryId &&
                                    m.DateReturned == null);

            loan.DateReturned = DateTime.Today;

            string feedbackString = REPAIR_SUCCESS;
            string colorString = "green";

            if (loanResult < 1)
            {
                feedbackString = LOAN_ERROR;
                colorString = "red";
            }
            else if (repairResult < 1)
            {
                feedbackString = REPAIR_ERROR;
                colorString = "red";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.InstrumentInventoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Feedback", new { feedback = feedbackString, color = colorString });
            }
            
            return RedirectToAction ("Feedback", new { feedback = feedbackString, color = colorString });
        }


        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstrumentInventoryId,EnrollmentId,DateLoaned,DateReturned")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollment, "EnrollmentId", "EnrollmentId", loan.EnrollmentId);
            ViewData["InstrumentInventoryId"] = new SelectList(_context.InstrumentInventory, "InstrumentInventoryId", "Manufacturer", loan.InstrumentInventoryId);
            return View(loan);
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int id1, int id2)
        {

            var loan = await _context.Loan
                .Include(l => l.Enrollment)
                .Include(l => l.InstrumentInventory)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id1 && m.InstrumentInventoryId == id2 && m.DateReturned == null);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Loan newLoan)
        {
            var loan = await _context.Loan
                .Include(l => l.Enrollment)
                .Include(l => l.InstrumentInventory)
                .FirstOrDefaultAsync
                (m => m.EnrollmentId == Convert.ToInt32(Request.Form["EnrollmentId"]) && 
                m.InstrumentInventoryId == Convert.ToInt32(Request.Form["InstrumentInventoryId"]) &&
                m.DateReturned == null
                );
            
            loan.DateReturned = DateTime.Now;

            string feedbackString = LOAN_RETURN_SUCCESS;
            string colorString = "green";

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.InstrumentInventoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Feedback", new { feedback = feedbackString, color = colorString });
            }
            ViewData["EnrollmentId"] = new SelectList(_context.Enrollment, "EnrollmentId", "EnrollmentId", loan.EnrollmentId);
            ViewData["InstrumentInventoryId"] = new SelectList(_context.InstrumentInventory, "InstrumentInventoryId", "Manufacturer", loan.InstrumentInventoryId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .Include(l => l.Enrollment)
                .Include(l => l.InstrumentInventory)
                .FirstOrDefaultAsync(m => m.InstrumentInventoryId == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loan.FindAsync(id);
            _context.Loan.Remove(loan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loan.Any(e => e.InstrumentInventoryId == id);
        }
    }
}
