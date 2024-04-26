namespace EngineeringExamPreparation.Models
{
        public class TestResultViewModel
        {
            public int Score { get; set; }

            public int QuestionCount { get; set; }
            public List<TestResultQuestionViewModel> Questions { get; set; }
        }

        public class TestResultQuestionViewModel
        {
            public string Text { get; set; }
            public string SelectedChoiceText { get; set; }
            public string CorrectChoiceText { get; set; }

        public List<TestChoice> testChoices { get; set; }
        }
}
