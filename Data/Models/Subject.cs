namespace Schedule.Data.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }         // Название предмета

        // Навигационное свойство — список занятий, по данному предмету
        public ICollection<Lesson> Lessons { get; set; }
    }

}
