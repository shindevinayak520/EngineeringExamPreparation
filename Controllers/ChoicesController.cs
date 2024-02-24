using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EngineeringExamPreparation.Models;
using EngineeringExamPreparation.Data;

namespace EngineeringExamPreparation.Controllers
{
    public class ChoicesController : Controller
    {
        private readonly EnggExamContext _context;

        public static IEnumerable<SelectListItem> QuestionList { get; set; }

        public ChoicesController(EnggExamContext context)
        {
            _context = context;
        }

        // GET: Choices
        public async Task<IActionResult> Index()
        {
              return _context.Choices != null ? 
                          View(await _context.Choices.ToListAsync()) :
                          Problem("Entity set 'EnggExamContext.Choice'  is null.");
        }

        // GET: Choices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Choices == null)
            {
                return NotFound();
            }

            var choice = await _context.Choices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (choice == null)
            {
                return NotFound();
            }

            return View(choice);
        }

        // GET: Choices/Create
        public IActionResult Create(int questionId)
        {
            var question = _context.Questions.FindAsync(questionId);

            return View(question);
        }

        // POST: Choices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,IsCorrect,QuestionId")] Choice choice)
        {
            if (ModelState.IsValid)
            {
                _context.Choices.Add(choice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(choice);
        }

        // GET: Choices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Choices == null)
            {
                return NotFound();
            }

            var choice = await _context.Choices.FindAsync(id);
            if (choice == null)
            {
                return NotFound();
            }
            return View(choice);
        }

        // POST: Choices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,IsCorrect,QuestionId")] Choice choice)
        {
            if (id != choice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Choices.Update(choice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoiceExists(choice.Id))
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
            return View(choice);
        }

        // GET: Choices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Choices == null)
            {
                return NotFound();
            }

            var choice = await _context.Choices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (choice == null)
            {
                return NotFound();
            }

            return View(choice);
        }

        // POST: Choices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Choices == null)
            {
                return Problem("Entity set 'EnggExamContext.Choice'  is null.");
            }
            var choice = await _context.Choices.FindAsync(id);
            if (choice != null)
            {
                _context.Choices.Remove(choice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChoiceExists(int id)
        {
          return (_context.Choices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
