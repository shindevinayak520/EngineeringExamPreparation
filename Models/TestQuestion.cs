using System.ComponentModel;

namespace EngineeringExamPreparation.Models
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public List<TestChoice> TestChoices { get; set; }

        [DefaultValue("Not-Submitted")]
        public string Status { get; set; }
    }
}
