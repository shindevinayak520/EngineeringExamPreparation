using System.ComponentModel;

namespace EngineeringExamPreparation.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int ChapterId { get; set; }
        public Chapter? Chapter { get; set; }

        public int NumberOfQuestion { get; set; }

        public List<TestQuestion>? TestQuestions { get; set; }
    }

}
