using System.ComponentModel.DataAnnotations;

namespace Schedule.Data.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string? Name { get; set; }         // Название предмета

        // Навигационное свойство — список занятий, по данному предмету
        public ICollection<Lesson>? Lessons { get; set; }
    }

}
