namespace EngineeringExamPreparation.Models
{
    public class TestChoice
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public bool Selected { get; set; }
        public int TestQuestionId { get; set; }
        public TestQuestion TestQuestion { get; set; }

    }
}

