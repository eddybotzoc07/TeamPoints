using Microsoft.AspNetCore.Identity;

namespace TeamPoints.Models
{
    public enum TipoTransaccion
    {
        Ganar = 1,
        Canjear = 2
    }

    public class TransaccionPuntos
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; } = string.Empty;
        public TipoTransaccion Tipo { get; set; }
        public int Puntos { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string? Ref { get; set; }
    }
}
