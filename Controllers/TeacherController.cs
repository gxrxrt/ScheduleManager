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
        public IActionResult Index(string searchString)
        {
            var teachers = _teacherRepository.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(t => t.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.SearchString = searchString; // 👈 Передаём строку в представление

            return View(teachers);
        }



        // Create
        public IActionResult Create()
        {
            return View(new Teacher());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher teacher, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "teachers");
                    Directory.CreateDirectory(uploadsFolder); // на случай если папка не существует

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }

                    teacher.PhotoPath = "/images/teachers/" + uniqueFileName;
                }

                _teacherRepository.Add(teacher);
                _teacherRepository.Save();
                return RedirectToAction("Index");
            }

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
        public IActionResult Edit(int id, Teacher teacher, IFormFile photo)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "teachers");
                    Directory.CreateDirectory(uploadsFolder); // на случай если папка не существует

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(photo.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }

                    teacher.PhotoPath = "/images/teachers/" + uniqueFileName;
                }
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
