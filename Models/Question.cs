namespace EngineeringExamPreparation.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<Choice>? Choices { get; set; }
        public int ChapterId { get; set; }
        public Chapter? Chapter { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
