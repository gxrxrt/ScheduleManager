using Microsoft.EntityFrameworkCore;
using Schedule.Data.Interfaces;
using Schedule.Data.Models;

namespace Schedule.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll() => _dbSet.ToList();

        public T GetById(int id) => _dbSet.Find(id);

        public void Add(T entity) => _dbSet.Add(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Save() => _context.SaveChanges();
    }

    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(AppDbContext context) : base(context) { }

        public override IEnumerable<Teacher> GetAll()
        {
            return _dbSet.Where(t => t.Name != null).ToList();
        }

        public Teacher GetTeacherByEmail(string email)
        {
            return _dbSet.FirstOrDefault(t => t.Email == email);
        }
    }


    public class ClassroomRepository : Repository<Classroom>, IClassroomRepository
    {
        public ClassroomRepository(AppDbContext context) : base(context) { }

        public override IEnumerable<Classroom> GetAll()
        {
            return _dbSet.Where(c => c.RoomNumber != null).ToList();
        }

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
        public SubjectRepository(AppDbContext context) : base(context) { }

        public override IEnumerable<Subject> GetAll()
        {
            return _dbSet.Where(s => s.Name != null).ToList();
        }
    }


    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context) : base(context) { }
    }

    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(AppDbContext context) : base(context) { }

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

        public Lesson GetByIdWithDetails(int id)
        {
            return _dbSet
                .Include(l => l.Teacher)
                .Include(l => l.Classroom)
                .Include(l => l.Subject)
                .Include(l => l.LessonGroups)
                    .ThenInclude(lg => lg.Group)
                .FirstOrDefault(l => l.Id == id);
        }

        public IEnumerable<Lesson> GetAllWithIncludes()
        {
            return _dbSet
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.Classroom)
                .Include(l => l.LessonGroups)
                    .ThenInclude(lg => lg.Group)
                .ToList();
        }

    }

    public class LessonGroupRepository : Repository<LessonGroup>, ILessonGroupRepository
    {
        public LessonGroupRepository(AppDbContext context) : base(context) { }
    }

    public class ScheduleRepository : Repository<Schedules>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext context) : base(context) { }

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
