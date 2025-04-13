using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Schedule.Data.Interfaces;
using Schedule.Data.Models;

namespace Schedule.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IClassroomRepository _classroomRepository;

        public LessonController(
            ILessonRepository lessonRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            IGroupRepository groupRepository,
            IClassroomRepository classroomRepository)
        {
            _lessonRepository = lessonRepository;
            _teacherRepository = teacherRepository;
            _subjectRepository = subjectRepository;
            _groupRepository = groupRepository;
            _classroomRepository = classroomRepository;
        }

        public IActionResult Index()
        {
            var lessons = _lessonRepository.GetAllWithIncludes();
            return View(lessons);
        }

        public IActionResult Details(int id)
        {
            var lesson = _lessonRepository.GetAllWithIncludes().FirstOrDefault(l => l.Id == id);
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        public IActionResult Create()
        {
            LoadSelectLists();
            return View(new Lesson());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Lesson lesson, int[] groupIds)
        {
            if (lesson.StartTime == default || lesson.EndTime == default)
            {
                ModelState.AddModelError("", "Дата и время начала/окончания занятия не могут быть пустыми.");
            }
            if (lesson.EndTime <= lesson.StartTime)
            {
                ModelState.AddModelError("", "Время окончания должно быть позже времени начала.");
            }

            if (ModelState.IsValid)
            {
                lesson.LessonGroups = groupIds.Select(id => new LessonGroup { GroupId = id }).ToList();

                if (_lessonRepository.CheckForConflicts(lesson))
                {
                    Console.WriteLine("Конфликт в расписании. Проверьте время, аудиторию, преподавателя и группы.");
                    ModelState.AddModelError("", "Конфликт в расписании. Проверьте время, аудиторию, преподавателя и группы.");
                }
                else
                {
                    _lessonRepository.Add(lesson);
                    _lessonRepository.Save();
                    return RedirectToAction("Index");
                }
            }
            // Повторно загружаем ViewBag-и
            ViewBag.Teachers = new SelectList(_teacherRepository.GetAll(), "Id", "Name", lesson.TeacherId);
            ViewBag.Subjects = new SelectList(_subjectRepository.GetAll(), "Id", "Name", lesson.SubjectId);
            ViewBag.Classrooms = new SelectList(_classroomRepository.GetAll(), "Id", "RoomNumber", lesson.ClassroomId);
            ViewBag.Groups = new MultiSelectList(_groupRepository.GetAll(), "Id", "Name", groupIds);

            return View(lesson);
        }


        public IActionResult Edit(int id)
        {
            var lesson = _lessonRepository.GetAllWithIncludes().FirstOrDefault(l => l.Id == id);
            if (lesson == null) return NotFound();

            LoadSelectLists(lesson);
            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Lesson lesson, int[] groupIds)
        {
            if (id != lesson.Id) return NotFound();

            if (groupIds.Length == 0)
                ModelState.AddModelError("", "Выберите хотя бы одну группу.");

            lesson.LessonGroups = groupIds.Select(gid => new LessonGroup { GroupId = gid, LessonId = lesson.Id }).ToList();

            if (_lessonRepository.CheckForConflicts(lesson))
                ModelState.AddModelError("", "Обнаружена коллизия в расписании.");
            if (lesson.StartTime == default || lesson.EndTime == default)
            {
                ModelState.AddModelError("", "Дата и время начала/окончания занятия не могут быть пустыми.");
            }
            if (lesson.EndTime <= lesson.StartTime)
            {
                ModelState.AddModelError("", "Время окончания должно быть позже времени начала.");
            }

            if (ModelState.IsValid)
            {
                _lessonRepository.Update(lesson);
                _lessonRepository.Save();
                return RedirectToAction(nameof(Index));
            }

            LoadSelectLists(lesson);
            return View(lesson);
        }

        public IActionResult Delete(int id)
        {
            var lesson = _lessonRepository.GetAllWithIncludes().FirstOrDefault(l => l.Id == id);
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var lesson = _lessonRepository.GetById(id);
            if (lesson != null)
            {
                _lessonRepository.Delete(lesson);
                _lessonRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        private void LoadSelectLists(Lesson? lesson = null)
        {
            // Если lesson равно null, используем дефолтные значения
            var teacherId = lesson?.TeacherId ?? 0; // Дефолтное значение 0, если null
            var subjectId = lesson?.SubjectId ?? 0; // Дефолтное значение 0, если null
            var classroomId = lesson?.ClassroomId ?? 0; // Дефолтное значение 0, если null

            // Загружаем данные для селектов
            ViewBag.Teachers = new SelectList(_teacherRepository.GetAll(), "Id", "Name", teacherId);
            ViewBag.Subjects = new SelectList(_subjectRepository.GetAll(), "Id", "Name", subjectId);
            ViewBag.Classrooms = new SelectList(_classroomRepository.GetAll(), "Id", "RoomNumber", classroomId);

            var allGroups = _groupRepository.GetAll();
            // Если lesson не null, выбираем ID групп из lesson, иначе передаем пустой список
            var selectedIds = lesson?.LessonGroups?.Select(lg => lg.GroupId) ?? new List<int>();

            // Создаем MultiSelectList
            ViewBag.Groups = new MultiSelectList(allGroups, "Id", "Name", selectedIds);
        }


    }
}
