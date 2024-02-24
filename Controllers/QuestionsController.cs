using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EngineeringExamPreparation.Models;
using Microsoft.VisualBasic;
using EngineeringExamPreparation.Data;

namespace EngineeringExamPreparation.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly EnggExamContext _context;

        public static IEnumerable<SelectListItem> ChapterList { get; set; }
        public QuestionsController(EnggExamContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var questions = _context.Questions.Include(q => q.Choices).ToList();
            return View(questions);
        }

        public IActionResult Create()
        {
            // Load dropdown list of chapters for selecting a chapter when creating a question
            ViewBag.Chapters = _context.Chapters.ToList();
            return View();
        }

        public IActionResult AddMultiple(int chapterId)
        {
            ViewBag.ChapterId = chapterId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMultiple(int chapterId, List<Question> questions)
        {
            if (ModelState.IsValid)
            {
                foreach (var question in questions)
                {
                    question.ChapterId = chapterId;
                    _context.Questions.Add(question);
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "Questions"); // Redirect to desired page
            }
            ViewBag.ChapterId = chapterId;
            return View(questions);
        }

        [HttpPost]
        public IActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                // Add question to the database
                _context.Questions.Add(question);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // Reload dropdown list of chapters for creating a question
            ViewBag.Chapters = _context.Chapters.ToList();
            return View(question);
        }

        // Add Edit, Details, Delete actions as needed
    

    // GET: Questions/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Chapters = _context.Chapters.ToList();

            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = _context.Questions.Include(q => q.Choices).FirstOrDefault(q => q.QuestionId == id);
            
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id , Question updatedQuestion)
        {
            if (id != updatedQuestion.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Retrieve the original question from the database
                var originalQuestion = _context.Questions
                    .Include(q => q.Choices)
                    .FirstOrDefault(q => q.QuestionId == updatedQuestion.QuestionId);

                if (originalQuestion == null)
                {
                    return NotFound();
                }

                // Update question details
                originalQuestion.QuestionText = updatedQuestion.QuestionText;
                originalQuestion.ChapterId = updatedQuestion.ChapterId;

                // Update choices
                foreach (var updatedChoice in updatedQuestion.Choices)
                {
                    var originalChoice = originalQuestion.Choices.FirstOrDefault(c => c.Id == updatedChoice.Id);

                    if (originalChoice == null)
                    {
                        // New choice, add it to the database
                        originalQuestion.Choices.Add(updatedChoice);
                    }
                    else
                    {
                        // Existing choice, update it
                        originalChoice.Text = updatedChoice.Text;
                        originalChoice.IsCorrect = updatedChoice.IsCorrect;
                    }
                }

                // Remove deleted choices
                foreach (var originalChoice in originalQuestion.Choices.ToList())
                {
                    if (!updatedQuestion.Choices.Any(c => c.Id == originalChoice.Id))
                    {
                        _context.Choices.Remove(originalChoice);
                    }
                }

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // Reload dropdown list of chapters for editing a question
            ViewBag.Chapters = _context.Chapters.ToList();
            return View(updatedQuestion);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Questions == null)
            {
                return Problem("Entity set 'EnggExamContext.Question'  is null.");
            }
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
          return (_context.Questions?.Any(e => e.QuestionId == id)).GetValueOrDefault();
        }
    }
}
