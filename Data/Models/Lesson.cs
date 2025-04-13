namespace Schedule.Data.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }  // Время начала занятия
        public DateTime EndTime { get; set; }    // Время окончания занятия

        // Внешний ключ для аудитории
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }

        // Внешний ключ для преподавателя
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        // Внешний ключ для предмета
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        // Связь многие ко многим с группами
        public ICollection<LessonGroup> LessonGroups { get; set; }
    }

}
