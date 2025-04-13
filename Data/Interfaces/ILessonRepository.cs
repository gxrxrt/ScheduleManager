using Schedule.Data.Models;

namespace Schedule.Data.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        // Метод для проверки коллизий при добавлении/редактировании занятия
        bool CheckForConflicts(Lesson lesson);

        // Метод для получения расписания по фильтрам (по преподавателю, группе, дате)
        IEnumerable<Lesson> GetSchedule(Func<Lesson, bool> predicate);
    }

}
