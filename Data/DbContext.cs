using Microsoft.EntityFrameworkCore;
using Schedule.Data.Models;

namespace Schedule.Data
{
    public class ScheduleDbContext : DbContext
    {
        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options) { }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonGroup> LessonGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LessonGroup>()
                .HasKey(lg => new { lg.LessonId, lg.GroupId });

            // При необходимости настройте связи, индексы, ограничения
        }
    }

}
