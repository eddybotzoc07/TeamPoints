using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamPoints.Data;
using TeamPoints.Models;

namespace TeamPoints.Controllers
{
    [Authorize(Roles = "Admin,RH")]
    public class PremiosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PremiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Premios.AsNoTracking().ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Premio premio)
        {
            if (!ModelState.IsValid) return View(premio);
            _context.Add(premio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var premio = await _context.Premios.FindAsync(id);
            if (premio == null) return NotFound();
            return View(premio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Premio premio)
        {
            if (id != premio.Id) return NotFound();
            if (!ModelState.IsValid) return View(premio);
            _context.Update(premio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var premio = await _context.Premios.FindAsync(id);
            if (premio == null) return NotFound();
            return View(premio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var premio = await _context.Premios.FindAsync(id);
            if (premio != null)
            {
                _context.Premios.Remove(premio);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
