using EngineeringExamPreparation.Models;
using Microsoft.EntityFrameworkCore;

namespace EngineeringExamPreparation.Data
{
    public class EnggExamContext : DbContext
    {

        public EnggExamContext(DbContextOptions<EnggExamContext> options)
            : base(options)
        {
        }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<TestChoice> TestChoices { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Comment> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships

            // Test - Chapter
            modelBuilder.Entity<Test>()
                .HasOne(t => t.Chapter)
                .WithMany(c => c.Tests)
                .HasForeignKey(t => t.ChapterId);

            // Question - Chapter
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Chapter)
                .WithMany(c => c.Questions)
                .HasForeignKey(q => q.ChapterId);

            // Choice - Question
            modelBuilder.Entity<Choice>()
                .HasOne(c => c.Question)
                .WithMany(q => q.Choices)
                .HasForeignKey(c => c.QuestionId);

            // TestQuestion - Test
            modelBuilder.Entity<TestQuestion>()
                .HasOne(tq => tq.Test)
                .WithMany(t => t.TestQuestions)
                .HasForeignKey(tq => tq.TestId);

            // TestChoice - TestQuestion
            modelBuilder.Entity<TestChoice>()
                .HasOne(tc => tc.TestQuestion)
                .WithMany(tq => tq.TestChoices)
                .HasForeignKey(tc => tc.TestQuestionId);

            // Branch - Subject
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Branch)
                .WithMany(b => b.Subjects)
                .HasForeignKey(s => s.BranchId);

            // Subject - Chapter
            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Chapters)
                .HasForeignKey(c => c.SubjectId);


            // Question - Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Question)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.QuestionId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
