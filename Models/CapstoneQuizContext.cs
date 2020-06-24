using Microsoft.EntityFrameworkCore;

namespace CapstoneQuizAPI.Models
{
    public class CapstoneQuizContext : DbContext
    {
        public CapstoneQuizContext(DbContextOptions<CapstoneQuizContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<ResultType> ResultType { get; set; }
        public DbSet<Topic> Topic { get; set; }
    }
}
