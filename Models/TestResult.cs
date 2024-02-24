namespace EngineeringExamPreparation.Models
{
    public class TestResult
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public int MarksObtained { get; set; }

        public int TotalQuestions { get; set; }
    }
}
