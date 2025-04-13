using Microsoft.AspNetCore.Mvc;
using Schedule.Data.Interfaces;
using Schedule.Data.Models;

namespace Schedule.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        // Index
        public IActionResult Index()
        {

            var teachers = _teacherRepository.GetAll();
            Console.WriteLine($"Teachers count: {teachers.Count()}"); // Логирование

            return View(teachers);
        }

        // Create
        public IActionResult Create()
        {
            return View(new Teacher());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher teacher)
        {
            Console.WriteLine("POST: Create() вызван");
            Console.WriteLine($"Имя: {teacher.Name}, Email: {teacher.Email}");

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState валиден");

                _teacherRepository.Add(teacher);
                _teacherRepository.Save();
                return RedirectToAction("Index");
            }
            Console.WriteLine("ModelState НЕ валиден");
            return View(teacher);
        }

        // Edit
        public IActionResult Edit(int id)
        {
            var teacher = _teacherRepository.GetById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _teacherRepository.Update(teacher);
                _teacherRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // Delete
        public IActionResult Delete(int id)
        {
            var teacher = _teacherRepository.GetById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var teacher = _teacherRepository.GetById(id);
            if (teacher != null)
            {
                _teacherRepository.Delete(teacher);
                _teacherRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
