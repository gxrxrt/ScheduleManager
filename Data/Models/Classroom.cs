using System.ComponentModel.DataAnnotations;

namespace Schedule.Data.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string? RoomNumber { get; set; }  // Номер или название аудитории

        [Required(ErrorMessage = "Вместимость обязательно")]
        public int? Capacity { get; set; }       // Вместимость аудитории

        // Навигационное свойство — список занятий, проводимых в данной аудитории
        public ICollection<Lesson>? Lessons { get; set; } = new List<Lesson>();
    }

}
