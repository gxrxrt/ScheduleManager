using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Schedule.Data;
using Schedule.Data.Models;
using System.Xml.Linq;

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
            ViewBag.CurrentId = groupId; // Добавлено для экспорта
            ViewBag.ScheduleType = "group";
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
            ViewBag.CurrentId = teacherId; // Добавлено для экспорта
            ViewBag.ScheduleType = "teacher";
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
        public IActionResult ExportToXml(string scheduleType, int? id = null)
        {
            var lessons = _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.LessonGroups) // Обязательно загружаем LessonGroups
                .ThenInclude(lg => lg.Group) // Убедитесь, что Group тоже загружается
                .AsQueryable();

            if (scheduleType == "group" && id.HasValue)
            {
                lessons = lessons.Where(l => l.LessonGroups.Any(g => g.GroupId == id));
            }
            else if (scheduleType == "teacher" && id.HasValue)
            {
                lessons = lessons.Where(l => l.TeacherId == id);
            }

            var lessonList = lessons.ToList();

            var xml = new XElement("Lessons",
                lessonList.Select(l =>
                    new XElement("Lesson",
                        new XElement("Subject", l.Subject.Name),
                        new XElement("Teacher", l.Teacher.Name),
                        new XElement("Classroom", l.Classroom.RoomNumber),
                        new XElement("StartTime", l.StartTime),
                        new XElement("EndTime", l.EndTime),
                        new XElement("Groups",
                            l.LessonGroups.Any() ?
                            l.LessonGroups.Select(g =>
                                new XElement("Group", g.Group.Name)
                            ) :
                            new XElement("Group", "No groups")
                        )
                    )
                )
            );

            var stream = new MemoryStream();
            xml.Save(stream);
            stream.Position = 0;

            return File(stream, "application/xml", "schedule.xml");
        }


    }
}
