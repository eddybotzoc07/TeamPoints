using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamPoints.Data;
using TeamPoints.Models;

namespace TeamPoints.Controllers
{
    [Authorize(Roles = "Admin,RH")]
    public class LogrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Logros.AsNoTracking().ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Logro logro)
        {
            if (!ModelState.IsValid) return View(logro);
            _context.Add(logro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var logro = await _context.Logros.FindAsync(id);
            if (logro == null) return NotFound();
            return View(logro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Logro logro)
        {
            if (id != logro.Id) return NotFound();
            if (!ModelState.IsValid) return View(logro);
            _context.Update(logro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var logro = await _context.Logros.FindAsync(id);
            if (logro == null) return NotFound();
            return View(logro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logro = await _context.Logros.FindAsync(id);
            if (logro != null)
            {
                _context.Logros.Remove(logro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
