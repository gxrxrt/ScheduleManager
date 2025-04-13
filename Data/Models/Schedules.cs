using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schedule.Data.Models
{
    public class Schedules
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }

        [Required]
        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public Teacher Teacher { get; set; }

        [Required]
        public int ClassroomId { get; set; }
        [ForeignKey(nameof(ClassroomId))]
        public Classroom Classroom { get; set; }

        [Required]
        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
