using Schedule.Data.Models;

namespace Schedule.Data.Interfaces
{
    public interface ILessonGroupRepository : IRepository<LessonGroup>
    {
        // Методы для работы со связью занятий и групп, если потребуется
    }

}
