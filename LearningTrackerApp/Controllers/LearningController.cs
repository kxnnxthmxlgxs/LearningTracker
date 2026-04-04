using LearningTrackerApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningTrackerApp.Controllers
{
    [Authorize] // This locks the entire controller!
    public class LearningController : Controller
    {
        private readonly LearningRepository _repo;

        public LearningController(LearningRepository repo)
        {
            _repo = repo;
        }

        // This shows the list
        public async Task<IActionResult> Index(string searchString)
        {
            // If searchString is null, our repository handles it as an empty string
            var items = await _repo.GetAllItems(searchString ?? "");


            // Get our new stats
            var stats = await _repo.GetStats();
            ViewBag.Total = stats.Total;
            ViewBag.Completed = stats.Completed;
            ViewBag.TopCategory = stats.TopCategory;

            // We store the search term so the search box stays filled after we click 'Search'
            ViewData["CurrentFilter"] = searchString;

            return View(items);
        }

        // This handles the "Add" button
        [HttpPost]
        public async Task<IActionResult> Add(string topic, string category)
        {
            if (!string.IsNullOrEmpty(topic))
            {
                await _repo.AddItem(topic, category);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteItem(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            await _repo.CompleteItem(id);
            return RedirectToAction("Index");
        }
    }
}