using EngineeringExamPreparation.Data;
using EngineeringExamPreparation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace EngineeringExamPreparation.Controllers
{
    public class McqTestController : Controller
    {
        private readonly EnggExamContext dbcontext;

        // Display the form for creating a MCQ test

        public McqTestController(EnggExamContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public ActionResult Index()
        {
            var chapters = dbcontext.Tests.Include(b=>b.TestQuestions).ToList();

            return View(chapters);
        }
        public ActionResult Create()
        {
            ViewBag.Chapters = new SelectList(dbcontext.Chapters, "ChapterId", "ChapterName");
            return View();
        }

        // Handle the creation of a MCQ test
        [HttpPost]
        public ActionResult Create(Test mcqTest)
        {
            var chapter = dbcontext.Chapters.Find(mcqTest.ChapterId);

            if (chapter == null)
            {
                ViewBag.Message = "Chapter not found.";
                return View("ChapterNotFound");
            }

            mcqTest.Title = $"MCQ Test for Chapter {chapter.ChapterName}";
            mcqTest.ChapterId = mcqTest.ChapterId;

            dbcontext.Tests.Add(mcqTest);
            dbcontext.SaveChanges();

            // Get random questions for the test
            var availableQuestions = dbcontext.Questions.Where(q => q.ChapterId == mcqTest.ChapterId).ToList();
            var randomQuestions = GetRandomQuestions(availableQuestions, mcqTest.NumberOfQuestion);

            // Associate questions with the test
            foreach (var question in randomQuestions)
            {
                var testQuestion = new TestQuestion
                {
                    Text = question.QuestionText,
                    TestId = mcqTest.Id
                };

                dbcontext.TestQuestions.Add(testQuestion);
                dbcontext.SaveChanges();
                // Associate choices with the test question
                var choices = dbcontext.Choices.Where(c => c.QuestionId == question.QuestionId).ToList();
                foreach (var choice in choices)
                {
                    var testChoice = new TestChoice
                    {
                        Text = choice.Text,
                        IsCorrect = choice.IsCorrect,
                        TestQuestionId = testQuestion.Id
                    };

                    dbcontext.TestChoices.Add(testChoice);
                    dbcontext.SaveChanges();
                }
            }

            dbcontext.SaveChanges();

            return RedirectToAction("Index");
        }

        // Helper method to get random questions
        private List<Question> GetRandomQuestions(List<Question> availableQuestions, int numberOfQuestions)
        {
            var random = new Random();
            return availableQuestions.OrderBy(q => random.Next()).Take(numberOfQuestions).ToList();
        }


        // GET: /Test/TakeTest
        public IActionResult TakeTest(int testId)
        {
            var questions = dbcontext.TestQuestions.Include(c=>c.TestChoices).Where(t=>t.TestId==testId).ToList();
            return View(questions);
        }

        // POST: /Test/SubmitTest
        [HttpPost]
        public IActionResult SubmitTest(List<TestQuestion> questions)
        {
            // Calculate the result
            int totalQuestions = questions.Count;
            int correctAnswers = 0;
            
            foreach (var question in questions)
            {
                var selectedChoices = question.TestChoices.Where(c => c.Selected);
                var correctAnswer = dbcontext.TestChoices.Where(t=>t.TestQuestionId==question.Id && t.IsCorrect);
                // Ensure there is exactly one selected choice for each question
                if (selectedChoices.Count() == 1 && selectedChoices.First().Id == correctAnswer.First().Id)
                {
                    correctAnswers++;
                }
            }

            var result = new TestResult();
            result.TotalQuestions = totalQuestions;
            result.MarksObtained = correctAnswers;
            return View("Result", result);
        }


        // GET: /Test/Result
        public IActionResult Result(TestResult result)
        {
            var testResult = new TestResult();
            // Retrieve the result from ViewBag and display it in the view
            testResult.TotalQuestions = result.TotalQuestions;
            testResult.MarksObtained = result.MarksObtained;
            return View(testResult);
        }




        // Display details of a test
        public ActionResult Details(int id)
        {
            var test = dbcontext.Tests.Find(id);

            if (test == null)
            {
                ViewBag.Message = "Test not found.";
                return View("TestNotFound");
            }

            return View(test);
        }

        // Dispose of the database context
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbcontext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
