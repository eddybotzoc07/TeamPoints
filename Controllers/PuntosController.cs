using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamPoints.Data;
using TeamPoints.Models;
using TeamPoints.Models.ViewModels;

namespace TeamPoints.Controllers
{
    [Authorize]
    public class PuntosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PuntosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,RH")]
        [HttpGet]
        public async Task<IActionResult> Asignar()
        {
            var users = await _userManager.Users.ToListAsync();
            var logros = await _context.Logros.OrderBy(l => l.Nombre).ToListAsync();
            var vm = new AsignarPuntosVm
            {
                Usuarios = users.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = u.Id, Text = u.Email ?? u.UserName ?? u.Id }).ToList(),
                Logros = logros.Select(l => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = l.Id.ToString(), Text = $"{l.Nombre} (+{l.Puntos})" }).ToList()
            };
            return View(vm);
        }

        [Authorize(Roles = "Admin,RH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Asignar(AsignarPuntosVm vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var logro = await _context.Logros.FindAsync(vm.LogroId);
            if (logro == null) return NotFound();

            var tx = new TransaccionPuntos
            {
                UsuarioId = vm.UsuarioId,
                Tipo = TipoTransaccion.Ganar,
                Puntos = logro.Puntos,
                Ref = $"LOGRO:{vm.LogroId}"
            };
            _context.Transacciones.Add(tx);
            await _context.SaveChangesAsync();
            TempData["Ok"] = "Puntos asignados";
            return RedirectToAction(nameof(Historial), new { usuarioId = vm.UsuarioId });
        }

        [HttpGet]
        public async Task<IActionResult> Canjear()
        {
            ViewBag.Premios = await _context.Premios.AsNoTracking().OrderBy(p => p.CostoPuntos).ToListAsync();
            var userId = _userManager.GetUserId(User)!;
            ViewBag.Balance = await ObtenerBalance(userId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Canjear(int premioId)
        {
            var premio = await _context.Premios.FindAsync(premioId);
            if (premio == null) return NotFound();

            var userId = _userManager.GetUserId(User)!;
            var balance = await ObtenerBalance(userId);
            if (balance < premio.CostoPuntos)
            {
                TempData["Error"] = "No tienes puntos suficientes";
                return RedirectToAction(nameof(Canjear));
            }

            var tx = new TransaccionPuntos
            {
                UsuarioId = userId,
                Tipo = TipoTransaccion.Canjear,
                Puntos = premio.CostoPuntos,
                Ref = $"PREMIO:{premioId}"
            };
            _context.Transacciones.Add(tx);
            await _context.SaveChangesAsync();
            TempData["Ok"] = "Canje realizado";
            return RedirectToAction(nameof(Historial));
        }

        [Authorize]
        public async Task<IActionResult> Historial(string? usuarioId = null)
        {
            usuarioId ??= _userManager.GetUserId(User)!;
            var txs = await _context.Transacciones
                .Where(t => t.UsuarioId == usuarioId)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
            ViewBag.Balance = await ObtenerBalance(usuarioId);
            return View(txs);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Leaderboard()
        {
            var since = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek); // start of week (Sunday)
            var leaderboard = await _context.Transacciones
                .Where(t => t.Tipo == TipoTransaccion.Ganar && t.Fecha >= since)
                .GroupBy(t => t.UsuarioId)
                .Select(g => new LeaderboardVm { UsuarioId = g.Key, Puntos = g.Sum(x => x.Puntos) })
                .OrderByDescending(x => x.Puntos)
                .Take(10)
                .ToListAsync();
            return View(leaderboard);
        }

        private async Task<int> ObtenerBalance(string usuarioId)
        {
            var ganados = await _context.Transacciones.Where(t => t.UsuarioId == usuarioId && t.Tipo == TipoTransaccion.Ganar).SumAsync(t => (int?)t.Puntos) ?? 0;
            var canjeados = await _context.Transacciones.Where(t => t.UsuarioId == usuarioId && t.Tipo == TipoTransaccion.Canjear).SumAsync(t => (int?)t.Puntos) ?? 0;
            return ganados - canjeados;
        }
    }

    public class LeaderboardVm
    {
        public string UsuarioId { get; set; } = string.Empty;
        public int Puntos { get; set; }
    }
}
