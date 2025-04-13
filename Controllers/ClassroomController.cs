using Microsoft.AspNetCore.Mvc;
using Schedule.Data.Interfaces;
using Schedule.Data.Models;

namespace Schedule.Controllers
{
    public class ClassroomController : Controller
    {
        private readonly IClassroomRepository _classroomRepository;

        public ClassroomController(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
        }

        // GET: Classroom/Index
        public IActionResult Index()
        {
            var classrooms = _classroomRepository.GetAll();
            return View(classrooms);
        }

        // GET: Classroom/Create
        public IActionResult Create()
        {
            return View(new Classroom());
        }

        // POST: Classroom/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                _classroomRepository.Add(classroom);
                _classroomRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(classroom);
        }

        // GET: Classroom/Edit/5
        public IActionResult Edit(int id)
        {
            var classroom = _classroomRepository.GetById(id);
            if (classroom == null)
            {
                return NotFound();
            }
            return View(classroom);
        }

        // POST: Classroom/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Classroom classroom)
        {
            if (id != classroom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _classroomRepository.Update(classroom);
                _classroomRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(classroom);
        }

        // GET: Classroom/Delete/5
        public IActionResult Delete(int id)
        {
            var classroom = _classroomRepository.GetById(id);
            if (classroom == null)
            {
                return NotFound();
            }
            return View(classroom);
        }

        // POST: Classroom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var classroom = _classroomRepository.GetById(id);
            if (classroom != null)
            {
                _classroomRepository.Delete(classroom);
                _classroomRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
