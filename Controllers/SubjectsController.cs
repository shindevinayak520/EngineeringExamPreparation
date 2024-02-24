using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EngineeringExamPreparation.Models;
using System.Runtime.CompilerServices;
using EngineeringExamPreparation.Data;

namespace EngineeringExamPreparation.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly EnggExamContext _context;

        public SubjectsController(EnggExamContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            var subjects = _context.Subjects.Include(x => x.Branch).Include(y=>y.Chapters).ToList();

            return View(subjects);
        }

        public IActionResult AddMultiple(int branchId)
        {
            ViewBag.BranchId = branchId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMultiple(int branchId, List<Subject> subjects)
        {
            if (ModelState.IsValid)
            {
                foreach (var subject in subjects)
                {
                    subject.BranchId = branchId;
                    _context.Subjects.Add(subject);
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "Subjects"); // Redirect to desired page
            }
            ViewBag.BranchId = branchId;
            return View(subjects);
        }


        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            // Reload dropdown list of chapters for creating a question
            ViewBag.Branches = _context.Branches.ToList();
            var subject = new Subject();
            subject.BranchId = 0;
            return View(subject);
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (ModelState.IsValid)
            {
                    _context.Subjects.Add(subject);
                    await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            // Reload dropdown list of chapters for creating a question
            ViewBag.Branches = _context.Branches.ToList();

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubjectId,SubjectName,BranchId,BranchName")] Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Subjects.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.SubjectId))
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
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subjects == null)
            {
                return Problem("Entity set 'EnggExamContext.Subject'  is null.");
            }
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
          return (_context.Subjects?.Any(e => e.SubjectId == id)).GetValueOrDefault();
        }
    }
}
