using EngineeringExamPreparation.Data;
using EngineeringExamPreparation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using static EngineeringExamPreparation.Models.TestResultViewModel;

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
            //var testQuestions = dbcontext.TestQuestions.Where(t=>t.TestId == testId).ToList();

            var viewModel = PrepareTestResultViewModel(questions);
            return View("Result", viewModel);
        }


        // Method to prepare the test result view model
        private TestResultViewModel PrepareTestResultViewModel(List<TestQuestion> submittedTest)
        {
            int score = 0;
            int questionCount = submittedTest.Count();
            List<TestResultQuestionViewModel> questions = new List<TestResultQuestionViewModel>();

            foreach (var question in submittedTest)
            {
                var correctAnswerId = dbcontext.TestChoices
                    .Where(c => c.TestQuestionId == question.Id && c.IsCorrect)
                    .Select(c => c.Id)
                    .FirstOrDefault();

                // Example logic: If the selected choice is correct, increase the score
                bool isCorrect = CheckIfCorrect(correctAnswerId, question);
                if (isCorrect)
                {
                    score++;
                }
                question.TestChoices.Find(t => t.Id == correctAnswerId).IsCorrect = true;

                // Create view model for each question
                TestResultQuestionViewModel questionViewModel = new TestResultQuestionViewModel
                {
                    Text = question.Text,
                    SelectedChoiceText = GetSelectedChoiceText(question),
                    CorrectChoiceText = GetCorrectChoiceText(question),
                    testChoices = question.TestChoices // Assuming TestChoices is a property in TestResultQuestionViewModel
                };

                questions.Add(questionViewModel);
            }

            return new TestResultViewModel
            {
                Score = score,
                QuestionCount = questionCount,
                Questions = questions
            };
        }


        // Example methods for getting selected and correct choice text
        private bool CheckIfCorrect(int correctAnswerId, TestQuestion question)
        {
            int? selectedChoiceId = question.SelectedChoiceId;

            // Ensure there is exactly one selected choice for each question
            if (selectedChoiceId.HasValue && selectedChoiceId.Value == correctAnswerId)
            {
                return true;
            }
            return false;
        }


        private string GetSelectedChoiceText(TestQuestion question)
        {
            int? selectedChoiceId = question.SelectedChoiceId;
            if (selectedChoiceId.HasValue)
            {
                var selectedChoice = question.TestChoices.FirstOrDefault(c => c.Id == selectedChoiceId);
                return selectedChoice?.Text ?? "No choice selected";
            }
            return "No choice selected";
        }

        private string GetCorrectChoiceText(TestQuestion question)
        {
            int? correctChoiceId = question.TestChoices.FirstOrDefault(c => c.IsCorrect)?.Id;
            if (correctChoiceId.HasValue)
            {
                var correctChoice = question.TestChoices.FirstOrDefault(c => c.Id == correctChoiceId);
                return correctChoice?.Text ?? "No correct choice found";
            }
            return "No correct choice found";
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
