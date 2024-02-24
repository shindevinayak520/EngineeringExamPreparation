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
    public class ChaptersController : Controller
    {
        private readonly EnggExamContext _context;
        public static IEnumerable<SelectListItem> SubjectList { get; set; }

        public ChaptersController(EnggExamContext context)
        {
            _context = context;
        }

        // GET: Chapters
        public async Task<IActionResult> Index()
        {
            var chapters = _context.Chapters.Include(x=>x.Subject).Include(y=>y.Questions).ToList();

            return View(chapters);
        }

        public IActionResult AddMultiple(int subjectId)
        {
            ViewBag.SubjectId = subjectId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMultiple(int subjectId, List<Chapter> chapters)
        {
            if (ModelState.IsValid)
            {
                foreach (var chapter in chapters)
                {
                    chapter.SubjectId = subjectId;
                    _context.Chapters.Add(chapter);
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "Chapters"); // Redirect to desired page
            }
            ViewBag.SubjectId = subjectId;
            return View(chapters);
        }

        // GET: Chapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chapters == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .FirstOrDefaultAsync(m => m.ChapterId == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // GET: Chapters/Create
        public IActionResult Create(int subjecId)
        {
            // Load dropdown list of chapters for selecting a chapter when creating a question
            ViewBag.Subjects = _context.Subjects.ToList();
            ViewBag.SubjectId = subjecId;
            return View();
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChapterId,ChapterName,SubjectId")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                _context.Chapters.Add(chapter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chapter);
        }

        // GET: Chapters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chapters == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChapterId,ChapterName,SubjectId")] Chapter chapter)
        {
            if (id != chapter.ChapterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Chapters.Update(chapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChapterExists(chapter.ChapterId))
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
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chapters == null)
            {
                return NotFound();
            }

            var chapter = await _context.Chapters
                .FirstOrDefaultAsync(m => m.ChapterId == id);
            if (chapter == null)
            {
                return NotFound();
            }

            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chapters == null)
            {
                return Problem("Entity set 'EnggExamContext.Chapter'  is null.");
            }
            var chapter = await _context.Chapters.FindAsync(id);
            if (chapter != null)
            {
                _context.Chapters.Remove(chapter);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChapterExists(int id)
        {
          return (_context.Chapters?.Any(e => e.ChapterId == id)).GetValueOrDefault();
        }
    }
}
