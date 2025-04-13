using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult GroupSchedule(int groupId, string sortField = "StartTime", string sortOrder = "asc")
        {
            var lessons = _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.LessonGroups)
                .Where(l => l.LessonGroups.Any(g => g.GroupId == groupId))
                .AsQueryable();

            lessons = ApplySorting(lessons, sortField, sortOrder);

            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            return PartialView("_SchedulePartial", lessons.ToList());
        }

        // Получение расписания для выбранного преподавателя
        public IActionResult TeacherSchedule(int teacherId, string sortField = "StartTime", string sortOrder = "asc")
        {
            var lessons = _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.LessonGroups)
                .Where(l => l.TeacherId == teacherId)
                .AsQueryable();

            lessons = ApplySorting(lessons, sortField, sortOrder);

            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            return PartialView("_SchedulePartial", lessons.ToList());
        }

        // Получение общего расписания с возможностью сортировки
        public IActionResult GeneralSchedule(string sortField = "StartTime", string sortOrder = "asc")
        {
            var lessons = _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.LessonGroups)
                .AsQueryable();

            lessons = ApplySorting(lessons, sortField, sortOrder);

            ViewBag.SortField = sortField;
            ViewBag.SortOrder = sortOrder;
            return PartialView("_SchedulePartial", lessons.ToList());
        }

        // Метод для применения сортировки
        private IQueryable<Lesson> ApplySorting(IQueryable<Lesson> lessons, string sortField, string sortOrder)
        {
            return sortField switch
            {
                "Teacher" => sortOrder == "asc"
                    ? lessons.OrderBy(l => l.Teacher.Name)
                    : lessons.OrderByDescending(l => l.Teacher.Name),

                "Subject" => sortOrder == "asc"
                    ? lessons.OrderBy(l => l.Subject.Name)
                    : lessons.OrderByDescending(l => l.Subject.Name),

                "Classroom" => sortOrder == "asc"
                    ? lessons.OrderBy(l => l.Classroom.RoomNumber)
                    : lessons.OrderByDescending(l => l.Classroom.RoomNumber),

                "EndTime" => sortOrder == "asc"
                    ? lessons.OrderBy(l => l.EndTime)
                    : lessons.OrderByDescending(l => l.EndTime),

                _ => sortOrder == "asc"
                    ? lessons.OrderBy(l => l.StartTime)
                    : lessons.OrderByDescending(l => l.StartTime),
            };
        }
    }
}
