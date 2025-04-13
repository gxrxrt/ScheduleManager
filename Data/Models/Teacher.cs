using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schedule.Data.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Специализация обязательна")]
        public string? Specialization { get; set; }

        [NotMapped]
        public ICollection<Lesson>? Lessons { get; set; } = new List<Lesson>();



    }


}
