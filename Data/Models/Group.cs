using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schedule.Data.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }      // Наименование группы или курса

        [Required(ErrorMessage = "Курс обязательно")]
        public string Course { get; set; }         // Курс
                                                   // Дополнительные поля: факультет, специальность и т.п.

        // Навигационное свойство — связь многие ко многим с занятиями
        [NotMapped]
        public ICollection<LessonGroup> LessonGroups { get; set; } = new List<LessonGroup>();
    }

}
