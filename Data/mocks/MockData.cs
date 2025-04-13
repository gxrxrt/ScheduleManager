using Schedule.Data.Models;

namespace Schedule.Data.Mocks
{
    public static class MockData
    {
        public static List<Teacher> GetTeachers() => new()
        {
            new Teacher { Id = 1, Name = "Иванов Иван", Email = "ivanov@example.com", Specialization = "Математика" },
            new Teacher { Id = 2, Name = "Петров Петр", Email = "petrov@example.com", Specialization = "Физика" }
        };

        public static List<Classroom> GetClassrooms() => new()
        {
            new Classroom { Id = 1, RoomNumber = "101", Capacity = 30 },
            new Classroom { Id = 2, RoomNumber = "202", Capacity = 25 }
        };

        public static List<Subject> GetSubjects() => new()
        {
            new Subject { Id = 1, Name = "Алгебра" },
            new Subject { Id = 2, Name = "Геометрия" }
        };

        public static List<Group> GetGroups() => new()
        {
            new Group { Id = 1, Name = "Группа 42919/6", Course = "4 курс" },
            new Group { Id = 2, Name = "Группа 32919/12", Course = "3 курс" }
        };

        public static List<Lesson> GetLessons()
        {
            return new List<Lesson>
            {
                new Lesson
                {
                    Id = 1,
                    StartTime = DateTime.Today.AddHours(9),
                    EndTime = DateTime.Today.AddHours(10.5),
                    ClassroomId = 1,
                    TeacherId = 1,
                    SubjectId = 1,
                    LessonGroups = new List<LessonGroup>()
                },
                new Lesson
                {
                    Id = 2,
                    StartTime = DateTime.Today.AddHours(11),
                    EndTime = DateTime.Today.AddHours(12.5),
                    ClassroomId = 2,
                    TeacherId = 2,
                    SubjectId = 2,
                    LessonGroups = new List<LessonGroup>()
                }
            };
        }

        public static List<LessonGroup> GetLessonGroups() => new()
        {
            new LessonGroup { LessonId = 1, GroupId = 1 },
            new LessonGroup { LessonId = 2, GroupId = 2 }
        };

        public static List<Schedules> GetSchedules() => new()
        {
            new Schedules
            {
                Id = 1,
                SubjectId = 1,
                TeacherId = 1,
                ClassroomId = 1,
                GroupId = 1,
                StartTime = DateTime.Today.AddHours(9),
                EndTime = DateTime.Today.AddHours(10.5),
                Date = DateTime.Today
            },
            new Schedules
            {
                Id = 2,
                SubjectId = 2,
                TeacherId = 2,
                ClassroomId = 2,
                GroupId = 2,
                StartTime = DateTime.Today.AddHours(11),
                EndTime = DateTime.Today.AddHours(12.5),
                Date = DateTime.Today
            }
        };
    }
}
