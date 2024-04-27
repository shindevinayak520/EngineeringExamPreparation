using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EngineeringExamPreparation.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EngineeringExamPreparation.Data;

namespace EngineeringExamPreparation.Controllers
{
    public class AdminController : Controller
    {
        private readonly EnggExamContext _dbContext;

        public AdminController(EnggExamContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult UploadQuestions()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadQuestions(IFormFile file)
        {
            if (file != null && file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                List<Question> questions = ExtractQuestionsFromExcel(file);

                foreach (var question in questions)
                {
                    _dbContext.Questions.Add(question);
                }
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Admin"); // Redirect to admin dashboard or appropriate page
            }
            else
            {
                ModelState.AddModelError("file", "Please upload a valid Excel file.");
                return View();
            }
        }

        private List<Question> ExtractQuestionsFromExcel(IFormFile file)
        {
            List<Question> questions = new List<Question>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {
                        int rowCount = worksheet.Dimension.Rows;
                        for (int i = 2; i <= rowCount; i++) // Assuming data starts from row 2 (header in row 1)
                        {
                            Question question = new Question
                            {
                                QuestionText = worksheet.Cells[i, 1].Value?.ToString(), // Assuming question text in column 1
                                Choices = new List<Choice>
                                {
                                    new Choice { Text = worksheet.Cells[i, 2].Value?.ToString(), IsCorrect = Convert.ToBoolean(worksheet.Cells[i, 3].Value) }, // Assuming choices in columns 2 and 3 (text and correctness)
                                    new Choice { Text = worksheet.Cells[i, 4].Value?.ToString(), IsCorrect = Convert.ToBoolean(worksheet.Cells[i, 5].Value) },
                                    new Choice { Text = worksheet.Cells[i, 6].Value?.ToString(), IsCorrect = Convert.ToBoolean(worksheet.Cells[i, 7].Value) },
                                    new Choice { Text = worksheet.Cells[i, 8].Value?.ToString(), IsCorrect = Convert.ToBoolean(worksheet.Cells[i, 9].Value) },
                                    // Add more choices as needed
                                },
                                ChapterId = Convert.ToInt32(worksheet.Cells[i, 10].Value?.ToString())
                                
                            };
                            questions.Add(question);
                        }
                    }
                }
            }

            return questions;
        }
    }
}
