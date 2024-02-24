namespace EngineeringExamPreparation.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
