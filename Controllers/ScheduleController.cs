using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // Не забудьте добавить пространство имен
using Schedule.Data;
using Schedule.Data.Models;

namespace Schedule.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;
        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        // Главная страница "Расписание"
        public IActionResult Index()
        {
            var model = new ScheduleViewModel
            {
                Groups = _context.Groups
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                    .ToList(),
                Teachers = _context.Teachers
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Name
                    })
                    .ToList()
            };

            return View(model);
        }

        // Получение расписания для выбранной группы
        public IActionResult GroupSchedule(int groupId)
        {
            var lessons = _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.LessonGroups) // Если требуется для фильтрации или отображения связанных данных группы
                .Where(l => l.LessonGroups.Any(lg => lg.GroupId == groupId))
                .ToList();

            return PartialView("_SchedulePartial", lessons);
        }

        // Получение расписания для выбранного преподавателя
        public IActionResult TeacherSchedule(int teacherId)
        {
            var lessons = _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Where(l => l.TeacherId == teacherId)
                .ToList();

            return PartialView("_SchedulePartial", lessons);
        }

        // Получение общего расписания
        public IActionResult GeneralSchedule()
        {
            var lessons = _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .ToList();

            return PartialView("_SchedulePartial", lessons);
        }
    }
}
