using Microsoft.EntityFrameworkCore;

namespace CapstoneQuizAPI.Models
{
    public class CapstoneQuizContext : DbContext
    {
        public CapstoneQuizContext(DbContextOptions<CapstoneQuizContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<QuestionPerformance>(qp =>
                {
                    qp.HasNoKey();
                });
        }

        public DbSet<User> User { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<ResultType> ResultType { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<SessionQuestion> SessionQuestion { get; set; }
        public DbSet<TestSession> TestSession { get; set; }
        public DbSet<QuestionPerformance> QuestionPerformance { get; set; }
    }
}
