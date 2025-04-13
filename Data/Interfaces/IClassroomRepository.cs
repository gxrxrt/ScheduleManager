using Schedule.Data.Models;

namespace Schedule.Data.Interfaces
{
    public interface IClassroomRepository : IRepository<Classroom>
    {
        // Например, метод для поиска свободных аудиторий на заданное время и вместимость
        IEnumerable<Classroom> FindAvailableClassrooms(DateTime startTime, DateTime endTime);

    }

}
