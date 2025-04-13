using Schedule.Data.Models;

namespace Schedule.Data.Interfaces
{
    public interface ITeacherRepository : IRepository<Teacher>
    {

        // Методы, специфичные для работы с преподавателями:
        Teacher GetTeacherByEmail(string email);
    }

}
