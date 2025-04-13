namespace Schedule.Data.Models
{
    public class LessonGroup
    {
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }

}
