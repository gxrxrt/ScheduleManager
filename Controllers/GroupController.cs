using Microsoft.AspNetCore.Mvc;
using Schedule.Data.Interfaces;
using Schedule.Data.Models;

namespace Schedule.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupRepository _groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        // Index - отображаем все группы
        public IActionResult Index()
        {
            var groups = _groupRepository.GetAll();
            return View(groups);
        }

        // Create - отображаем форму для добавления новой группы
        public IActionResult Create()
        {
            return View(new Group());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Group group)
        {
            Console.WriteLine("POST: Create() вызван");
            Console.WriteLine($"Имя: {group.Name}, Курс: {group.Course}");
            if (!ModelState.IsValid)
            {

            }
            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState валиден");

                _groupRepository.Add(group);
                _groupRepository.Save();
                return RedirectToAction("Index");
            }
            Console.WriteLine("Invalid");
            return View(group);
        }

        // Edit - отображаем форму для редактирования группы
        public IActionResult Edit(int id)
        {
            var group = _groupRepository.GetById(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Group group)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _groupRepository.Update(group);
                _groupRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        // Delete - отображаем информацию о группе перед удалением
        public IActionResult Delete(int id)
        {
            var group = _groupRepository.GetById(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var group = _groupRepository.GetById(id);
            if (group != null)
            {
                _groupRepository.Delete(group);
                _groupRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
