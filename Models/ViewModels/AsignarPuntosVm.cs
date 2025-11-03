using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TeamPoints.Models.ViewModels
{
    public class AsignarPuntosVm
    {
        public string UsuarioId { get; set; } = string.Empty;
        public int LogroId { get; set; }
        public List<SelectListItem> Usuarios { get; set; } = new();
        public List<SelectListItem> Logros { get; set; } = new();
    }
}
