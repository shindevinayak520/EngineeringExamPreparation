namespace EngineeringExamPreparation.Models
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public string ChapterName { get; set; }

        // Foreign key to reference the Subject
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public List<Question>? Questions { get; set; }
        public List<Test>? Tests { get; set; }

    }
}
