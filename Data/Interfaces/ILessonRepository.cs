using Schedule.Data.Models;

namespace Schedule.Data.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        // Метод для проверки коллизий при добавлении/редактировании занятия
        bool CheckForConflicts(Lesson lesson);

        // Метод для получения расписания по фильтрам (по преподавателю, группе, дате)
        IEnumerable<Lesson> GetSchedule(Func<Lesson, bool> predicate);

        //метод для получения занятия по ID с вложенными сущностями
        Lesson GetByIdWithDetails(int id);

        //Метод GetAllWithIncludes() нужен для того, чтобы загружать Lesson с полными навигационными свойствами: Teacher, Subject, Classroom, LessonGroups → Group
        IEnumerable<Lesson> GetAllWithIncludes();

    }

}
