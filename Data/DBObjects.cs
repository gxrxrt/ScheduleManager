using Schedule.Data.Models;

namespace Schedule.Data
{
    public class DBObjects
    {
        public static void Initial(IServiceProvider serviceProvider, IHostEnvironment hostEnvironment)
        {
            var context = serviceProvider.GetRequiredService<ScheduleDbContext>();

            if (!context.Teachers.Any())
            {
                context.Teachers.AddRange(Teachers.Select(c => c.Value));
                context.SaveChanges();
            }
            if (!context.Groups.Any())
            {
                context.Groups.AddRange(Groups.Select(c => c.Value));
                context.SaveChanges();
            }

        }
        private static Dictionary<int, Group> group;
        public static Dictionary<int, Group> Groups
        {
            get
            {
                if (group == null)
                {
                    var list = new Group[]
                    {
                        new Group { Id = 1, Name = "Группа 42919/6", Course = "4 курс" },
                        new Group { Id = 2, Name = "Группа 32919/12", Course = "3 курс" }
                    };
                    group = new Dictionary<int, Group>();
                    foreach (var item in list)
                        group.Add(item.Id, item);
                }
                return group;
            }
        }
        private static Dictionary<int, Teacher> teacher;
        public static Dictionary<int, Teacher> Teachers
        {
            get
            {
                if (teacher == null)
                {
                    var list = new Teacher[]
                    {
                new Teacher { Id = 1, Name = "Иванов Иван", Email = "ivanov@example.com", Specialization = "Математика" },
                new Teacher { Id = 2, Name = "Петров Петр", Email = "petrov@example.com", Specialization = "Физика" }
                    };

                    teacher = new Dictionary<int, Teacher>();
                    foreach (var item in list)
                        teacher.Add(item.Id, item);
                }
                return teacher;
            }
        }

    }
}
