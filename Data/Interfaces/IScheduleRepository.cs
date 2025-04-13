
using Schedule.Data.Models;

namespace Schedule.Data.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedules>
    {
        // Метод для получения расписания группы по дате
        IEnumerable<Schedules> GetGroupSchedule(int groupId, DateTime date);

        // Метод для получения расписания преподавателя
        IEnumerable<Schedules> GetTeacherSchedule(int teacherId, DateTime date);
    }
}
