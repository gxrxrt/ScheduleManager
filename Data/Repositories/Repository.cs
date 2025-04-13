using Microsoft.EntityFrameworkCore;
using Schedule.Data.Interfaces;
using Schedule.Data.Models;

namespace Schedule.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ScheduleDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ScheduleDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T GetById(int id) => _dbSet.Find(id);

        public void Add(T entity) => _dbSet.Add(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Save() => _context.SaveChanges();
    }

    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(ScheduleDbContext context) : base(context) { }

        public Teacher GetTeacherByEmail(string email)
            => _dbSet.FirstOrDefault(t => t.Email == email);
    }

    public class ClassroomRepository : Repository<Classroom>, IClassroomRepository
    {
        public ClassroomRepository(ScheduleDbContext context) : base(context) { }

        public IEnumerable<Classroom> FindAvailableClassrooms(DateTime startTime, DateTime endTime)
        {
            var unavailableClassroomIds = _context.Lessons
                .Where(l => l.StartTime < endTime && l.EndTime > startTime)
                .Select(l => l.ClassroomId)
                .Distinct()
                .ToList();

            return _dbSet.Where(c => !unavailableClassroomIds.Contains(c.Id)).ToList();
        }
    }

    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(ScheduleDbContext context) : base(context) { }
    }

    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(ScheduleDbContext context) : base(context) { }
    }

    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(ScheduleDbContext context) : base(context) { }

        public bool CheckForConflicts(Lesson lesson)
        {
            bool classroomConflict = _context.Lessons.Any(l =>
                l.Id != lesson.Id &&
                l.ClassroomId == lesson.ClassroomId &&
                l.StartTime < lesson.EndTime &&
                l.EndTime > lesson.StartTime);
            if (classroomConflict) return true;

            bool teacherConflict = _context.Lessons.Any(l =>
                l.Id != lesson.Id &&
                l.TeacherId == lesson.TeacherId &&
                l.StartTime < lesson.EndTime &&
                l.EndTime > lesson.StartTime);
            if (teacherConflict) return true;

            foreach (var lg in lesson.LessonGroups)
            {
                bool groupConflict = _context.LessonGroups.Any(existingLg =>
                    existingLg.GroupId == lg.GroupId &&
                    existingLg.LessonId != lesson.Id &&
                    _context.Lessons.Any(l => l.Id == existingLg.LessonId &&
                                              l.StartTime < lesson.EndTime &&
                                              l.EndTime > lesson.StartTime));
                if (groupConflict) return true;
            }

            return false;
        }

        public IEnumerable<Lesson> GetSchedule(Func<Lesson, bool> predicate)
        {
            return _dbSet
                .Include(l => l.Teacher)
                .Include(l => l.Classroom)
                .Include(l => l.Subject)
                .Include(l => l.LessonGroups)
                    .ThenInclude(lg => lg.Group)
                .Where(predicate)
                .ToList();
        }
    }

    public class LessonGroupRepository : Repository<LessonGroup>, ILessonGroupRepository
    {
        public LessonGroupRepository(ScheduleDbContext context) : base(context) { }
    }

    public class ScheduleRepository : Repository<Schedules>, IScheduleRepository
    {
        public ScheduleRepository(ScheduleDbContext context) : base(context) { }

        public IEnumerable<Schedules> GetGroupSchedule(int groupId, DateTime date)
        {
            return _dbSet
                .Where(s => s.GroupId == groupId && s.Date.Date == date.Date)
                .ToList();
        }

        public IEnumerable<Schedules> GetTeacherSchedule(int teacherId, DateTime date)
        {
            return _dbSet
                .Where(s => s.TeacherId == teacherId && s.Date.Date == date.Date)
                .ToList();
        }
    }
}
