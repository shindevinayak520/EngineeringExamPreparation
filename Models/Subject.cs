namespace EngineeringExamPreparation.Models
{
    public class Subject
    {

            public int SubjectId { get; set; }
            public string SubjectName { get; set; }
        
            // Foreign key to reference the Branch
            public int BranchId { get; set; }
            public Branch? Branch { get; set; }

            // Navigation property to represent the relationship with chapters
            public List<Chapter>? Chapters { get; set; }
    }
}
